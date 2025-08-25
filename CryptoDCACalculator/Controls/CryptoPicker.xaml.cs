using System.Collections;

namespace CryptoDCACalculator.Controls
{
    public partial class CryptoPicker : VerticalStackLayout
    {
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CryptoPicker),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnSelectedItemPropertyChanged);

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(CryptoPicker),
                defaultValue: -1,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexPropertyChanged);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CryptoPicker),
                defaultValue: string.Empty,
                propertyChanged: OnPlaceholderPropertyChanged);

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(CryptoPicker),
                defaultValue: string.Empty,
                propertyChanged: OnTitlePropertyChanged);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CryptoPicker),
                defaultValue: null,
                propertyChanged: OnItemsSourcePropertyChanged);

        public static readonly BindableProperty InnerPaddingProperty =
            BindableProperty.Create(nameof(InnerPadding), typeof(Thickness), typeof(CryptoPicker),
                defaultValue: new Thickness(0),
                propertyChanged: OnInnerPaddingPropertyChanged);

        public CryptoPicker()
        {
            InitializeComponent();
            ItemDisplayBinding = null;
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        public BindingBase ItemDisplayBinding
        {
            get => InnerPicker.ItemDisplayBinding;
            set
            {
                if (InnerPicker.ItemDisplayBinding != null)
                    return;

                if (value is Binding binding)
                    InnerPicker.ItemDisplayBinding = new Binding(binding.Path);
                else
                    InnerPicker.ItemDisplayBinding = value;

                OnPropertyChanged();
            }
        }

        private static void OnSelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoPicker)bindable;
            if (control.InnerPicker != null)
            {
                control.InnerPicker.SelectedItem = newValue;
            }
            // Update display text
            control.UpdateDisplayText();
        }

        private static void OnSelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoPicker)bindable;
            if (control.InnerPicker != null)
            {
                control.InnerPicker.SelectedIndex = (int)newValue;
            }
            // Update display text
            control.UpdateDisplayText();
        }

        private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoPicker)bindable;
            var placeholder = (string)newValue;

            // Set the placeholder to the label text
            if (control.Children.Count > 0 && control.Children[0] is Label label)
            {
                label.Text = placeholder;
            }
        }

        private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoPicker)bindable;
            if (control.InnerPicker != null)
            {
                control.InnerPicker.Title = (string)newValue;
            }
        }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoPicker)bindable;
            if (control.InnerPicker != null)
            {
                control.InnerPicker.ItemsSource = (IList)newValue;
            }
        }

        private static void OnInnerPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoPicker)bindable;
            if (control.InnerEntry != null)
            {
                control.InnerEntry.Padding = (Thickness)newValue;
            }
        }

        private void UpdateDisplayText()
        {
            if (InnerEntry == null) return;

            string displayText = string.Empty;

            if (SelectedItem != null)
            {
                if (ItemDisplayBinding != null)
                {
                    // Try to get display text using binding
                    try
                    {
                        var bindingContext = SelectedItem;
                        if (ItemDisplayBinding is Binding binding)
                        {
                            // This is a simplified approach - in a real scenario you might need more complex binding resolution
                            var property = bindingContext.GetType().GetProperty(binding.Path);
                            if (property != null)
                            {
                                displayText = property.GetValue(bindingContext)?.ToString() ?? string.Empty;
                            }
                        }
                    }
                    catch
                    {
                        displayText = SelectedItem.ToString();
                    }
                }
                else
                {
                    displayText = SelectedItem.ToString();
                }
            }

            InnerEntry.Text = displayText;
        }

        private void OnPickerTapped(object sender, EventArgs e)
        {
            // Programmatically focus the hidden Picker to show the picker
            InnerPicker?.Focus();
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            // Ensure properties are synced when the control is loaded
            if (InnerPicker != null && InnerEntry != null)
            {
                InnerPicker.SelectedItem = SelectedItem;
                InnerPicker.SelectedIndex = SelectedIndex;
                InnerPicker.Title = Title;
                InnerPicker.ItemsSource = ItemsSource;
                InnerPicker.ItemDisplayBinding = ItemDisplayBinding;
                InnerEntry.Padding = InnerPadding;

                // Update display text
                UpdateDisplayText();

                // Set up event handler for selection changes
                InnerPicker.SelectedIndexChanged += (s, e) =>
                {
                    SelectedIndex = ItemsSource.IndexOf(InnerPicker.SelectedItem);
                    SelectedItem = InnerPicker.SelectedItem;
                };

                // Handle tap on ExtendedEntry to show picker
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += OnPickerTapped;
                InnerEntry.GestureRecognizers.Add(tapGesture);
            }
        }
    }
}
