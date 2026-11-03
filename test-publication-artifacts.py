#!/usr/bin/env python3
"""Exercise the actual workflow guard against contaminated package outputs."""

import os
from pathlib import Path
import subprocess
import tempfile
import unittest
import zipfile


WORKFLOW = Path(__file__).parent / ".github/workflows/publish-nuget.yaml"
SOURCE = WORKFLOW.read_text()
GUARD = SOURCE.split("      - name: Verify publication package\n", 1)[1].split("      - name:", 1)[0]
GUARD = "\n".join(line[10:] for line in GUARD.split("        run: |\n", 1)[1].splitlines())


class PublicationTests(unittest.TestCase):
	def run_guard(self, packages, expected_success):
		with tempfile.TemporaryDirectory() as directory:
			root = Path(directory)
			(root / "artifacts").mkdir()
			# Legacy packages may remain in an immutable historical checkout.
			(root / "artifacts/RaiUtilsCore.2.3.1.nupkg").write_bytes(b"legacy")
			output = root / "fresh-output"
			output.mkdir()
			for filename, package_id, version in packages:
				with zipfile.ZipFile(output / filename, "w") as archive:
					archive.writestr("package.nuspec", f'<package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"><metadata><id>{package_id}</id><version>{version}</version></metadata></package>')
			result = subprocess.run(["bash", "-eu", "-o", "pipefail", "-c", GUARD], cwd=root,
				env={**os.environ, "RELEASE_REF": "v4.5.17", "PACKAGE_OUTPUT": str(output)}, capture_output=True, text=True)
			self.assertEqual(expected_success, result.returncode == 0, result.stdout + result.stderr)

	def test_legacy_checkout_artifacts_are_excluded(self):
		self.run_guard([("RaiUtils.4.5.17.nupkg", "RaiUtils", "4.5.17")], True)

	def test_unexpected_package_in_output_is_rejected(self):
		self.run_guard([("RaiUtils.4.5.17.nupkg", "RaiUtils", "4.5.17"), ("RaiUtilsCore.2.3.1.nupkg", "RaiUtilsCore", "2.3.1")], False)

	def test_old_version_is_rejected(self):
		self.run_guard([("RaiUtils.2.3.1.nupkg", "RaiUtils", "2.3.1")], False)

	def test_wrong_embedded_identity_is_rejected(self):
		self.run_guard([("RaiUtils.4.5.17.nupkg", "RaiUtilsCore", "4.5.17")], False)

	def test_wrong_embedded_version_is_rejected(self):
		self.run_guard([("RaiUtils.4.5.17.nupkg", "RaiUtils", "2.3.1")], False)

	def test_missing_package_is_rejected(self):
		self.run_guard([], False)

	def test_publish_has_no_wildcard_and_uses_isolated_output(self):
		publish = SOURCE.split("      - name: Publish to NuGet\n", 1)[1]
		self.assertIn('dotnet nuget push "$PACKAGE_OUTPUT/RaiUtils.${RELEASE_REF#v}.nupkg"', publish)
		self.assertNotIn("*.nupkg", publish)
		self.assertIn('--output "$PACKAGE_OUTPUT"', SOURCE)
		self.assertIn('PACKAGE_OUTPUT="$RUNNER_TEMP/raiutils-nuget-$GITHUB_RUN_ID-$GITHUB_RUN_ATTEMPT"', SOURCE)
		self.assertIn('>> "$GITHUB_ENV"', SOURCE)
		self.assertLess(SOURCE.index("name: Verify publication package"), SOURCE.index("uses: NuGet/login@v1"))


if __name__ == "__main__":
	unittest.main()
