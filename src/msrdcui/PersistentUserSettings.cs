using System.Collections.Generic;

namespace msrdcui
{
    internal static class PersistentUserSettings
    {
        private const int MaxHistoryLength = 10;
        private const string RemoteComputerHistoryItemFormatString = "RemoteComputerHistoryItem{0:00}";

        public static IReadOnlyList<string> ReadRemoteComputerHistory()
        {
            var remoteComputerHistory = new List<string>(MaxHistoryLength);
            for (int i = 0; i < MaxHistoryLength; i++)
            {
                var propertyName = string.Format(RemoteComputerHistoryItemFormatString, i + 1);
                var historyItem = Properties.Settings.Default[propertyName] as string;
                if (!string.IsNullOrWhiteSpace(historyItem))
                {
                    remoteComputerHistory.Add(historyItem);
                }
            }
            return remoteComputerHistory;
        }

        public static void SaveRemoteComputerHistory(string latestRemoteComputer, IReadOnlyList<string> currentRemoteComputerHistory)
        {
            var newRemoteComputerHistory = BuildNewRemoteComputerHistory(latestRemoteComputer, currentRemoteComputerHistory);
            for (int i = 0; i < newRemoteComputerHistory.Count; i++)
            {
                var propertyName = string.Format(RemoteComputerHistoryItemFormatString, i + 1);
                Properties.Settings.Default[propertyName] = newRemoteComputerHistory[i];
            }
            Properties.Settings.Default.Save();
        }

        private static IReadOnlyList<string> BuildNewRemoteComputerHistory(string latestRemoteComputer, IReadOnlyList<string> currentRemoteComputerHistory)
        {
            var newRemoteComputerHistory = new List<string>(MaxHistoryLength)
            {
                latestRemoteComputer
            };

            int MaxCarryOveredItemCount = MaxHistoryLength - 1;
            int carryOveredItemCount = 0;
            for (int i = 0; i < currentRemoteComputerHistory.Count; i++)
            {
                if (currentRemoteComputerHistory[i] != latestRemoteComputer)
                {
                    newRemoteComputerHistory.Add(currentRemoteComputerHistory[i]);
                    carryOveredItemCount++;
                }
                if (carryOveredItemCount >= MaxCarryOveredItemCount) break;
            }

            return newRemoteComputerHistory;
        }
    }
}
