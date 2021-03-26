using System.Collections.Generic;

namespace rdclauncher
{
    internal static class PersistentUserSettings
    {
        private const int MaxHistoryLength = 10;

        // Remote computer name history

        private const string RemoteComputerHistoryItemFormatString = "RemoteComputerHistoryItem{0:00}";

        public static IReadOnlyList<string> ReadRemoteComputerHistory()
        {
            return ReadHistory(MaxHistoryLength, RemoteComputerHistoryItemFormatString);
        }

        public static void SaveRemoteComputerHistory(string latestRemoteComputer, IReadOnlyList<string> currentRemoteComputerHistory)
        {
            SaveHistory(latestRemoteComputer, currentRemoteComputerHistory, MaxHistoryLength, RemoteComputerHistoryItemFormatString);
        }

        public static void ClearRemoteComputerHistory()
        {
            ClearHistory(MaxHistoryLength, RemoteComputerHistoryItemFormatString);
        }

        // MSRSC window title history

        private const string RdcWindowTitleHistoryItemFormatString = "RdcWindowTitleHistoryItem{0:00}";

        public static IReadOnlyList<string> ReadRdcWindowTitleHistory()
        {
            return ReadHistory(MaxHistoryLength, RdcWindowTitleHistoryItemFormatString);
        }

        public static void SaveRdcWindowTitleHistory(string latestRdcWindowTitle, IReadOnlyList<string> currentRdcWindowTitleHistory)
        {
            SaveHistory(latestRdcWindowTitle, currentRdcWindowTitleHistory, MaxHistoryLength, RdcWindowTitleHistoryItemFormatString);
        }

        public static void ClearRdcWindowTitleHistory()
        {
            ClearHistory(MaxHistoryLength, RdcWindowTitleHistoryItemFormatString);
        }

        // Common

        private static IReadOnlyList<string> ReadHistory(int maxHistoryLength, string historyItemFormatString)
        {
            var history = new List<string>(maxHistoryLength);
            for (int i = 0; i < maxHistoryLength; i++)
            {
                var propertyName = string.Format(historyItemFormatString, i + 1);
                var historyItem = Properties.Settings.Default[propertyName] as string;
                if (!string.IsNullOrWhiteSpace(historyItem))
                {
                    history.Add(historyItem);
                }
            }
            return history;
        }

        private static void SaveHistory(string latestHistoryItem, IReadOnlyList<string> currentHistory, int maxHistoryLength, string historyItemFormatString)
        {
            var newHistory = BuildNewHistory(latestHistoryItem, currentHistory, maxHistoryLength);
            for (int i = 0; i < newHistory.Count; i++)
            {
                var propertyName = string.Format(historyItemFormatString, i + 1);
                Properties.Settings.Default[propertyName] = newHistory[i];
            }
            Properties.Settings.Default.Save();
        }

        private static IReadOnlyList<string> BuildNewHistory(string latestHistoryItem, IReadOnlyList<string> currentHistory, int maxHistoryLength)
        {
            var newHistory = new List<string>(maxHistoryLength)
            {
                latestHistoryItem
            };

            int maxCarryOveredItemCount = maxHistoryLength - 1;
            int carryOveredItemCount = 0;
            for (int i = 0; i < currentHistory.Count; i++)
            {
                if (currentHistory[i] != latestHistoryItem)
                {
                    newHistory.Add(currentHistory[i]);
                    carryOveredItemCount++;
                }
                if (carryOveredItemCount >= maxCarryOveredItemCount) break;
            }

            return newHistory;
        }

        private static void ClearHistory(int maxHistoryLength, string historyItemFormatString)
        {
            for (int i = 0; i < maxHistoryLength; i++)
            {
                var propertyName = string.Format(historyItemFormatString, i + 1);
                Properties.Settings.Default[propertyName] = string.Empty;
            }
            Properties.Settings.Default.Save();
        }
    }
}
