using System;
using System.Windows;
using System.Windows.Controls;

namespace rdclauncher
{
    public static class EditableComboBoxSpecialBehaviour
    {
        public static readonly DependencyProperty SelectTextBoxEntireTextAtFirstTimeProperty =
            DependencyProperty.RegisterAttached(
                "SelectTextBoxEntireTextAtFirstTime",
                typeof(bool),
                typeof(EditableComboBoxSpecialBehaviour),
                new UIPropertyMetadata(OnSelectTextBoxEntireTextAtFirstTimeChanged));

        public static bool GetSelectTextBoxEntireTextAtFirstTime(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectTextBoxEntireTextAtFirstTimeProperty);
        }

        public static void SetSelectTextBoxEntireTextAtFirstTime(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectTextBoxEntireTextAtFirstTimeProperty, value);
        }

        private static void OnSelectTextBoxEntireTextAtFirstTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is ComboBox comboBox)) return;

            var propertyValue = (bool)args.NewValue;
            if (propertyValue)
            {
                comboBox.Initialized += ComboBox_Initialized;
            }
        }

        private static void ComboBox_Initialized(object sender, EventArgs e)
        {
            if (!(sender is ComboBox comboBox)) return;

            var textBox = GetEditableTextBoxInComboBox(comboBox);
            if (textBox != null)
            {
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private static TextBox GetEditableTextBoxInComboBox(ComboBox comboBox)
        {
            comboBox.ApplyTemplate();
            return comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            // Select the text only when it is changed programmatically (e.g. binding).
            // Not select the text it is input by user even if that change is first time.
            if (textBox.CaretIndex == 0)
            {
                // Select entire text
                textBox.SelectionStart = 0;
                textBox.SelectionLength = textBox.Text.Length;
            }

            // Remove this event because this event fired at first time only.
            textBox.TextChanged -= TextBox_TextChanged;
        }
    }
}
