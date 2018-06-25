
using imbSCI.Core.data;
using System;
using Xamarin.Forms;

namespace imbACE.Generator.Xamarin.Core.data
{


    public static class settingsMemberInfoExtensions
    {



        public static Grid GetInputGrid(this settingsEntriesForObject objectInfo, String groupName = "")
        {
            var displayGrid = new Grid();

            

            var groupDict = objectInfo.GetMemberInfoGroupDictionary();

            if (groupName == "")
            {

            }
            else
            {
                if (groupDict.ContainsKey(groupName))
                {
                    groupDict = groupDict.GetSubsection(groupName);
                }
            }

            // building row definitions
            foreach (var group in groupDict.Values)
            {
                displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // for the group name
                foreach (var groupProps in group)
                {
                    displayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }
            }

            displayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Absolute) });
            displayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            displayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Absolute) });

            Int32 right = displayGrid.ColumnDefinitions.Count - 1;

            Int32 row = 0;
            foreach (var group in groupDict.Values)
            {
                Label groupLabel = new Label();
                groupLabel.Text = group.GroupName;
                displayGrid.Children.Add(groupLabel, 0, right, row, row+1);
                row++;

                foreach (var groupProps in group)
                {
                    Label letterLabel = new Label();
                    letterLabel.Text = groupProps.letter;
                    displayGrid.Children.Add(letterLabel, 0, row);


                    Entry valueEntry = new Entry();
                    valueEntry.Placeholder = groupProps.displayName;
                    valueEntry.Text = objectInfo.spes[groupProps.name].value.ToString();
                    displayGrid.Children.Add(valueEntry, 1, row);

                    Label unitLabel = new Label();
                    unitLabel.Text = groupProps.unit;
                    displayGrid.Children.Add(unitLabel, 2, row);

                    row++;
                }

            }

            return displayGrid;
        }


        public static Label GetLetterLabel(this settingsMemberInfoEntry memberEntry)
        {

            Label output = new Label();
            output.Text = memberEntry.letter;

            return output;

        }
    }
}
