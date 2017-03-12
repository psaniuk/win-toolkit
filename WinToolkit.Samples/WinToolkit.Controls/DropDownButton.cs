using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace WinToolkit.Controls
{
    public class DropDownButton : Control
    {
        private FontIcon _dropDownIcon;
        private FlyoutBase _flyout;
        private ListView _optionsList;
        private FrameworkElement _rootGrid;
        private TextBlock _selectedItemTextBlock;
        private const string DownUnicode = "\uE0E5";
        private const string UpUnicode = "\uE0E4";

        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(DropDownButton), new PropertyMetadata(default(object)));
        public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(DropDownButton), new PropertyMetadata(string.Empty));
        public static DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(DropDownButton), new PropertyMetadata(default(Style)));
        public static DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DropDownButton), new PropertyMetadata(default(DataTemplate)));
        public static DependencyProperty EmptySelectedItemTextProperty = DependencyProperty.Register("EmptySelectedItemText", typeof(string), typeof(DropDownButton), new PropertyMetadata(string.Empty));
        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(DropDownButton), new PropertyMetadata(default(object)));

        public DropDownButton()
        {
            DefaultStyleKey = typeof(DropDownButton);
            Tapped += DropDownButtonTapped;
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }

            set { SetValue(ItemTemplateProperty, value); }
        }

        public Style HeaderStyle
        {
            get { return (Style) GetValue(HeaderStyleProperty); }

            set { SetValue(HeaderStyleProperty, value); }
        }

        public string EmptySelectedItemText
        {
            get { return (string) GetValue(EmptySelectedItemTextProperty); }

            set { SetValue(EmptySelectedItemTextProperty, value); }
        }


        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }

            set { SetValue(ItemsSourceProperty, value); }
        }

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }

            set { SetValue(HeaderProperty, value); }
        }

        public void ShowOptionsList()
        {
            if (_rootGrid != null)
                FlyoutBase.ShowAttachedFlyout(_rootGrid);
        }

        public void HideOptionsList() => _flyout?.Hide();
 
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_flyout != null)
            {
                _flyout.Opened -= FlyoutOpened;
                _flyout.Closed -= FlyoutClosed;
            }

            _rootGrid = GetTemplateChild("RootGrid") as FrameworkElement;
            if (_rootGrid != null)
            {
                _flyout = FlyoutBase.GetAttachedFlyout(_rootGrid);
                if (_flyout != null)
                {
                    _flyout.Opened += FlyoutOpened;
                    _flyout.Closed += FlyoutClosed;
                }
            }

            _dropDownIcon = GetTemplateChild("DropDownIcon") as FontIcon;

            if (_optionsList != null)
                _optionsList.SelectionChanged -= OptionsListSelectionChanged;

            _optionsList = GetTemplateChild("OptionsList") as ListView;
            if (_optionsList != null)
                _optionsList.SelectionChanged += OptionsListSelectionChanged;

            _selectedItemTextBlock = GetTemplateChild("SelectedItemTextBlock") as TextBlock;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            ChangeState("PointerDown");
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            ChangeState("PointerUp");
        }

        private void ChangeState(string stateName) => VisualStateManager.GoToState(this, stateName, true);

        private void DropDownButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_rootGrid != null)
                FlyoutBase.ShowAttachedFlyout(_rootGrid);
        }

        private void FlyoutClosed(object sender, object e)
        {
            if (_dropDownIcon != null)
                _dropDownIcon.Glyph = DownUnicode;
        }

        private void FlyoutOpened(object sender, object e)
        {
            if (_dropDownIcon != null)
                _dropDownIcon.Glyph = UpUnicode;
        }

        private void OptionsListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = e.AddedItems?.FirstOrDefault();

            if (_selectedItemTextBlock != null)
                _selectedItemTextBlock.Text = SelectedItem != null ? SelectedItem.ToString() : EmptySelectedItemText;

            _flyout?.Hide();
        }
    }
}