namespace CryptoDCACalculator.Controls
{
    public partial class CryptoEntry : VerticalStackLayout
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CryptoEntry),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnTextPropertyChanged);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CryptoEntry),
                defaultValue: string.Empty,
                propertyChanged: OnPlaceholderPropertyChanged);

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(CryptoEntry),
                defaultValue: false,
                propertyChanged: OnIsPasswordPropertyChanged);

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(CryptoEntry),
                defaultValue: Keyboard.Default,
                propertyChanged: OnKeyboardPropertyChanged);

        public static readonly BindableProperty InnerPaddingProperty =
            BindableProperty.Create(nameof(InnerPadding), typeof(Thickness), typeof(CryptoEntry),
                defaultValue: new Thickness(0),
                propertyChanged: OnInnerPaddingPropertyChanged);

        public CryptoEntry()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoEntry)bindable;
            if (control.InnerEntry != null)
            {
                control.InnerEntry.Text = (string)newValue;
            }
        }

        private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoEntry)bindable;
            var placeholder = (string)newValue;

            // Set the placeholder to the label text
            if (control.Children.Count > 0 && control.Children[0] is Label label)
            {
                label.Text = placeholder;
            }
        }

        private static void OnIsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoEntry)bindable;
            if (control.InnerEntry != null)
            {
                control.InnerEntry.IsPassword = (bool)newValue;
            }
        }

        private static void OnKeyboardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoEntry)bindable;
            if (control.InnerEntry != null)
            {
                control.InnerEntry.Keyboard = (Keyboard)newValue;
            }
        }

        private static void OnInnerPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CryptoEntry)bindable;
            if (control.InnerEntry != null)
            {
                control.InnerEntry.Padding = (Thickness)newValue;
            }
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            // Ensure properties are synced when the control is loaded
            if (InnerEntry != null)
            {
                InnerEntry.Text = Text;
                InnerEntry.IsPassword = IsPassword;
                InnerEntry.Keyboard = Keyboard;

                // Set up two-way binding for Text property
                InnerEntry.TextChanged += (s, e) => Text = e.NewTextValue;
            }
        }
    }
}
