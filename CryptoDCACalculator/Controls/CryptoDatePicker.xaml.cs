using Microsoft.Maui.Handlers;

namespace CryptoDCACalculator.Controls
{
    public partial class CryptoDatePicker : VerticalStackLayout
    {
        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(CryptoDatePicker),
                defaultValue: DateTime.Now,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnDatePropertyChanged);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CryptoDatePicker),
                defaultValue: string.Empty,
                propertyChanged: OnPlaceholderPropertyChanged);

        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(CryptoDatePicker),
                defaultValue: new DateTime(1900, 1, 1),
                propertyChanged: OnMinimumDatePropertyChanged);

        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(CryptoDatePicker),
                defaultValue: new DateTime(2100, 12, 31),
                propertyChanged: OnMaximumDatePropertyChanged);

        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(nameof(Format), typeof(string), typeof(CryptoDatePicker),
                defaultValue: "d",
                propertyChanged: OnFormatPropertyChanged);

        public static readonly BindableProperty InnerPaddingProperty =
            BindableProperty.Create(nameof(InnerPadding), typeof(Thickness), typeof(CryptoDatePicker),
                defaultValue: new Thickness(0),
                propertyChanged: OnInnerPaddingPropertyChanged);

        public CryptoDatePicker()
        {
            InitializeComponent();
            InnerDatePicker.MinimumDate = new DateTime(2020, 1, 1);
        }

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public DateTime MinimumDate
        {
            get => (DateTime)GetValue(MinimumDateProperty);
            set => SetValue(MinimumDateProperty, value);
        }

        public DateTime MaximumDate
        {
            get => (DateTime)GetValue(MaximumDateProperty);
            set => SetValue(MaximumDateProperty, value);
        }

        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        private static void OnDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoDatePicker)bindable;
            var newDate = (DateTime)newValue;

            if (control.InnerDatePicker != null)
            {
                control.InnerDatePicker.Date = newDate;
            }

            // Update the display text in ExtendedEntry
            control.UpdateDisplayText();
        }

        private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoDatePicker)bindable;
            var placeholder = (string)newValue;

            // Set the placeholder to the label text
            if (control.Children.Count > 0 && control.Children[0] is Label label)
            {
                label.Text = placeholder;
            }
        }

        private static void OnMinimumDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoDatePicker)bindable;
            if (control.InnerDatePicker != null)
            {
                control.InnerDatePicker.MinimumDate = (DateTime)newValue;
            }
        }

        private static void OnMaximumDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoDatePicker)bindable;
            if (control.InnerDatePicker != null)
            {
                control.InnerDatePicker.MaximumDate = (DateTime)newValue;
            }
        }

        private static void OnFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoDatePicker)bindable;
            if (control.InnerDatePicker != null)
            {
                control.InnerDatePicker.Format = (string)newValue;
            }
            // Update display text with new format
            control.UpdateDisplayText();
        }

        private static void OnInnerPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoDatePicker)bindable;
            if (control.InnerEntry != null)
            {
                control.InnerEntry.Padding = (Thickness)newValue;
            }
        }

        private void UpdateDisplayText()
        {
            if (InnerEntry != null && Date != DateTime.MinValue)
            {
                try
                {
                    InnerEntry.Text = Date.ToString(Format);
                }
                catch
                {
                    InnerEntry.Text = Date.ToString("d");
                }
            }
        }

        private void OnDatePickerTapped(object sender, EventArgs e)
        {
            // Programmatically focus the hidden DatePicker to show the picker
            InnerDatePicker?.Focus();
#if ANDROID
            var handler = InnerDatePicker.Handler as DatePickerHandler;
            handler.PlatformView.PerformClick();
#endif
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            // Ensure properties are synced when the control is loaded
            if (InnerDatePicker != null && InnerEntry != null)
            {
                InnerDatePicker.Date = Date;
                InnerDatePicker.MinimumDate = MinimumDate;
                InnerDatePicker.MaximumDate = MaximumDate;
                InnerDatePicker.Format = Format;
                InnerEntry.Padding = InnerPadding;

                // Update display text
                UpdateDisplayText();

                // Set up event handler for date selection
                InnerDatePicker.DateSelected += (s, e) =>
                {
                    Date = e.NewDate;
                };

                // Handle tap on ExtendedEntry to show picker
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += OnDatePickerTapped;
                InnerEntry.GestureRecognizers.Add(tapGesture);
            }
        }
    }
}
