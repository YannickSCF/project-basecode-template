/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/09/2023
 **/

#if UNITY_EDITOR
/// Dependencies
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.Text.RegularExpressions;

public class VersionController : IPreprocessBuildWithReport {

    private const int VERSION_LENGHT = 3;
    private const string VERSION_SEPARATOR = ".";
    private const string VERSION_SPECIFIC_INIT = "(";
    private const string VERSION_SPECIFIC_END = ")";
    private const string VERSION_DATE_SEPARATOR = " - ";
    private const string VERSION_DATE_FORMAT = "ddMMyy";

    private const string REGEX_VERSION_WITH_DATE = "^\\d+\\.\\d+\\.\\d+\\s-\\s\\d+$";
    private const string REGEX_VERSION_WITH_SPECIFIC_AND_DATE = "^\\d+\\.\\d+\\.\\d+\\(\\d+\\)\\s-\\s\\d+$";

    private enum VersionIndex {
        Major = 0,
        Minor = 1,
        Build = 2
    }

    #region IPreprocessBuildWithReport implemented methods
    public void OnPreprocessBuild(BuildReport report) {
        IncreaseBuild(report);
    }

    public int callbackOrder { get { return 0; } }
    #endregion

    #region Version methods
    [MenuItem("Build/Increase Major Version")]
    private static void IncreaseMajor() {
        IncrementVersion(VersionIndex.Major);
        Debug.Log("Major version increased: " + PlayerSettings.bundleVersion);
    }

    [MenuItem("Build/Increase Minor Version")]
    private static void IncreaseMinor() {
        IncrementVersion(VersionIndex.Minor);
        Debug.Log("Minor version increased: " + PlayerSettings.bundleVersion);
    }

    private static void IncrementVersion(VersionIndex versionToIncrement) {
        int indexToIncrement = (int)versionToIncrement;
        // Get current version
        int[] version = GetCurrentVersion();
        // Increment version requested
        version[indexToIncrement]++;
        // Set other version data
        switch (versionToIncrement) {
            case VersionIndex.Major:
                version[(int)VersionIndex.Minor] = 0;
                version[(int)VersionIndex.Build] = 0;
                break;
            case VersionIndex.Minor:
                version[(int)VersionIndex.Build] = 0;
                break;
            case VersionIndex.Build:
            default: break;
        }

        // Set version to settings
        PlayerSettings.bundleVersion = CreateVersionString(version);
        Debug.Log("New version setted to: " + PlayerSettings.bundleVersion);
    }

    private static int[] GetCurrentVersion() {
        int[] versionInts = new int[VERSION_LENGHT];

        // Check has standard length. VERSION_LENGHT values separated by VERSION_SEPARATOR
        string[] versionSplited = GetVersionWithoutExtraData();
        if (versionSplited.Length == VERSION_LENGHT) {
            // ... if it has the correct length: parse al version numbers
            for (int i = 0; i < VERSION_LENGHT; ++i) {
                // Try to parse the version part and, if it is an integer...
                if (int.TryParse(versionSplited[i], out int numberValue)) {
                    // .. save the integer
                    versionInts[i] = numberValue;
                } else {
                    // ... but, if it has any other thing: return default version
                    return GetDefaultVersion();
                }
            }
        } else {
            // ... but, if it has not the standard length: return default version
            return GetDefaultVersion();
        }

        return versionInts;
    }

    private static string[] GetVersionWithoutExtraData() {
        string onlyVersionStr = string.Empty;
        string originalVersion = PlayerSettings.bundleVersion;

        // In case it is a version just with date...
        Match regexMatch = Regex.Match(originalVersion, REGEX_VERSION_WITH_DATE);
        if (regexMatch.Success) {
            //... remove date
            onlyVersionStr = originalVersion.Split(VERSION_DATE_SEPARATOR)[0];
        } else {
            // In case it is not, check that is a version with specific version and date
            regexMatch = Regex.Match(originalVersion, REGEX_VERSION_WITH_SPECIFIC_AND_DATE);
            if (regexMatch.Success) {
                //... if it is remove specific version and date
                onlyVersionStr = originalVersion.Split(VERSION_SPECIFIC_INIT)[0];
            }
        }

        // If at this point version string is still null, set original string given
        if (string.IsNullOrEmpty(onlyVersionStr)) {
            onlyVersionStr = originalVersion;
        }
        // Always split version by VERSION_SEPARATOR at the end
        return onlyVersionStr.Split(VERSION_SEPARATOR);
    }

    private static int[] GetDefaultVersion() {
        int[] versionInts = new int[VERSION_LENGHT];

        for (int i = 0; i < VERSION_LENGHT; ++i) {
            versionInts[i] = 0;
        }

        return versionInts;
    }
    
    private static string CreateVersionString(int[] versionIntegers) {
        string finalVersion = versionIntegers[0].ToString();

        for (int i = 1; i < VERSION_LENGHT; ++i) {
            finalVersion += VERSION_SEPARATOR + versionIntegers[i];
        }

        string dateTimeStr = DateTime.Now.ToString(VERSION_DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        finalVersion += VERSION_DATE_SEPARATOR + dateTimeStr;

        return finalVersion;
    }
    #endregion

    #region Build methods
    private static void IncreaseBuild(BuildReport report) {
        // Increment build version number
        IncrementVersion(VersionIndex.Build);
        // Update Specific platform build number
        UpdatePlatformBuildNumber(report);
        Debug.Log("Build version increased: " + PlayerSettings.bundleVersion);
    }

    private static void UpdatePlatformBuildNumber(BuildReport report) {

        switch (report.summary.platform) {
            case BuildTarget.StandaloneOSX:
                PlayerSettings.macOS.buildNumber =
                    IncrementBuildNumber(PlayerSettings.macOS.buildNumber);
                UpdateBundleVersionWithSpecific(PlayerSettings.macOS.buildNumber);
                break;
            case BuildTarget.Android:
                ++PlayerSettings.Android.bundleVersionCode;
                UpdateBundleVersionWithSpecific(PlayerSettings.Android.bundleVersionCode.ToString());
                break;
            case BuildTarget.iOS:
                PlayerSettings.iOS.buildNumber =
                    IncrementBuildNumber(PlayerSettings.iOS.buildNumber);
                UpdateBundleVersionWithSpecific(PlayerSettings.iOS.buildNumber);
                break;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.WebGL:
            default:
                // Those build targets use the generic PlayerSettings.bundleVersion
                // to set the build version.
                // The build targets not setted will do the same, for now...
                break;
        }
    }

    private static string IncrementBuildNumber(string buildNumber) {
        int.TryParse(buildNumber, out int intBuildNumber);
        return (++intBuildNumber).ToString();
    }

    private static void UpdateBundleVersionWithSpecific(string buildNumber) {
        string newBundleVersion = PlayerSettings.bundleVersion;
        int initDateIndex = newBundleVersion.IndexOf(VERSION_DATE_SEPARATOR);

        newBundleVersion = newBundleVersion.Substring(0, initDateIndex) +
            VERSION_SPECIFIC_INIT + buildNumber + VERSION_SPECIFIC_END +
            newBundleVersion.Substring(initDateIndex);

        PlayerSettings.bundleVersion = newBundleVersion;
    }
    #endregion
}
#endif
