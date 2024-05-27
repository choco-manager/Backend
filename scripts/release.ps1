# Step 1: Get the new version with `git cliff --bumped-version`
$newVersion = & git cliff --bumped-version

Write-Host $newVersion

# Step 2: Strip "v" prefix from its output
$versionWithoutPrefix = $newVersion -replace '^v', ''

# Path to the csproj file
$csprojPath = "Choco.Backend.Api/Choco.Backend.Api.csproj"

# Load the XML file
[xml]$csprojXml = Get-Content $csprojPath

# Find the Version element and update its value
$versionElement = $csprojXml.SelectSingleNode("/Project/PropertyGroup/Version")
$versionElement.InnerText = $versionWithoutPrefix

# Save the updated XML file
$csprojXml.Save($csprojPath)

# Step 3: Create a commit named "chore(release): ${version-without-prefix}" with commit signing enabled
& git add $csprojPath
$commitMessage = "chore(release): $versionWithoutPrefix"
& git commit -S -m $commitMessage

# Step 4: Tag this commit with the tag named with prefix "v" (original output of step 1)
& git tag -a $newVersion -m $newVersion

# Push the commit and the tag
& git up --follow-tags
