# Step 1: Get the new version with `git cliff --bumped-version`
$newVersion = & git cliff --bumped-version

# Step 2: Strip "v" prefix from its output
$versionWithoutPrefix = $newVersion -replace '^v', ''

# Path to the csproj file
$csprojPath = "Choco.Backend.Api/Choco.Backend.Api.csproj"

# Load the XML file
[xml]$csprojXml = Get-Content $csprojPath

# Find the Version element and update its value
$versionElement = $csprojXml.Project.PropertyGroup.Version
if ($versionElement) {
    $versionElement.InnerText = $versionWithoutPrefix
} else {
    # If the Version element doesn't exist, create it
    $propertyGroup = $csprojXml.Project.PropertyGroup | Where-Object { $_.Version -ne $null }
    if (-not $propertyGroup) {
        $propertyGroup = $csprojXml.CreateElement("PropertyGroup")
        $csprojXml.Project.AppendChild($propertyGroup)
    }
    $versionElement = $csprojXml.CreateElement("Version")
    $versionElement.InnerText = $versionWithoutPrefix
    $propertyGroup.AppendChild($versionElement)
}

# Save the updated XML file
$csprojXml.Save($csprojPath)

# Step 3: Create a commit named "chore(release): ${version-without-prefix}" with commit signing enabled
& git add $csprojPath
$commitMessage = "chore(release): $versionWithoutPrefix"
& git commit -S -m $commitMessage

# Step 4: Tag this commit with the tag named with prefix "v" (original output of step 1)
& git tag -a $newVersion -m $newVersion

# Push the commit and the tag
& git push origin main --follow-tags
