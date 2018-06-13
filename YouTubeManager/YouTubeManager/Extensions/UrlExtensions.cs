﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace YouTubeManager.Extensions
{
    internal static class UrlExtensions
    {
        public static Dictionary<string, string> SplitQuery(string query)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var rawParams = query.Split("&");
            foreach (var rawParam in rawParams)
            {
                var param = WebUtility.UrlDecode(rawParam);

                // Look for the equals sign
                var equalsPos = param.IndexOf('=');
                if (equalsPos <= 0)
                    continue;

                // Get the key and value
                var key = param.Substring(0, equalsPos);
                var value = equalsPos < param.Length
                    ? param.Substring(equalsPos + 1)
                    : string.Empty;

                // Add to dictionary
                dictionary[key] = value;
            }

            return dictionary;
        }

        public static string SetQueryParameter(string url, string key, string value)
        {
            value = value ?? string.Empty;

            // Find existing parameter
            var existingMatch = Regex.Match(url, $@"[?&]({Regex.Escape(key)}=?.*?)(?:&|/|$)");

            // Parameter already set to something
            if (existingMatch.Success)
            {
                var group = existingMatch.Groups[1];

                // Remove existing
                url = url.Remove(group.Index, group.Length);

                // Insert new one
                url = url.Insert(group.Index, $"{key}={value}");

                return url;
            }
            // Parameter hasn't been set yet
            else
            {
                // See if there are other parameters
                var hasOtherParams = url.IndexOf('?') >= 0;

                // Prepend either & or ? depending on that
                var separator = hasOtherParams ? '&' : '?';

                // Assemble new query string
                return url + separator + key + '=' + value;
            }
        }

        public static string SetRouteParameter(string url, string key, string value)
        {
            value = value ?? string.Empty;

            // Find existing parameter
            var existingMatch = Regex.Match(url, $@"/({Regex.Escape(key)}/?.*?)(?:/|$)");

            // Parameter already set to something
            if (existingMatch.Success)
            {
                var group = existingMatch.Groups[1];

                // Remove existing
                url = url.Remove(group.Index, group.Length);

                // Insert new one
                url = url.Insert(group.Index, $"{key}/{value}");

                return url;
            }
            // Parameter hasn't been set yet
            else
            {
                // Assemble new query string
                return url + '/' + key + '/' + value;
            }
        }
    }
}
