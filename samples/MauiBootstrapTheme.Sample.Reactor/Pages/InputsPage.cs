using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class InputsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Input Controls").H1(),
                    Label("Form inputs with Bootstrap styling").Lead().Muted()
                ),

                // Text Inputs
                VStack(spacing: 12,
                    Label("Text Inputs").H4(),
                    VStack(spacing: 4,
                        Label("Default Input"),
                        Entry().Placeholder("Enter text...")
                    ),
                    VStack(spacing: 4,
                        Label("Large Input"),
                        Entry().Placeholder("Large input").Large()
                    ),
                    VStack(spacing: 4,
                        Label("Small Input"),
                        Entry().Placeholder("Small input").Small()
                    )
                ),

                // Textarea
                VStack(spacing: 12,
                    Label("Textarea").H4(),
                    VStack(spacing: 4,
                        Label("Default Textarea"),
                        Editor().Placeholder("Enter multi-line text...").HeightRequest(100)
                    )
                ),

                // Select
                VStack(spacing: 12,
                    Label("Select").H4(),
                    VStack(spacing: 4,
                        Label("Default Select"),
                        Picker().Title("Choose an option...").ItemsSource(new[] { "Option 1", "Option 2", "Option 3" })
                    )
                ),

                // Date & Time
                VStack(spacing: 12,
                    Label("Date & Time").H4(),
                    Grid("Auto", "*,*",
                        VStack(spacing: 4,
                            Label("Date"),
                            DatePicker()
                        ).GridColumn(0),
                        VStack(spacing: 4,
                            Label("Time"),
                            TimePicker()
                        ).GridColumn(1)
                    ).ColumnSpacing(20)
                ),

                // Checkboxes & Switches
                VStack(spacing: 12,
                    Label("Checkboxes & Switches").H4(),
                    HStack(spacing: 8,
                        CheckBox().IsChecked(true).Primary(),
                        Label("Default checkbox").VCenter()
                    ),
                    HStack(spacing: 8,
                        CheckBox().Primary(),
                        Label("Unchecked checkbox").VCenter()
                    ),
                    HStack(spacing: 8,
                        Switch().IsToggled(true).Primary(),
                        Label("Enabled switch").VCenter()
                    ),
                    HStack(spacing: 8,
                        Switch().Primary(),
                        Label("Disabled switch").VCenter()
                    )
                ),

                // Radio Buttons
                VStack(spacing: 12,
                    Label("Radio Buttons").H4(),
                    VStack(
                        RadioButton().Content("Option 1").IsChecked(true),
                        RadioButton().Content("Option 2"),
                        RadioButton().Content("Option 3")
                    ).Set(Microsoft.Maui.Controls.RadioButtonGroup.GroupNameProperty, "radioGroup")
                ),

                // Range Slider
                VStack(spacing: 12,
                    Label("Range Slider").H4(),
                    VStack(spacing: 4,
                        Label("Default Range"),
                        Slider().Minimum(0).Maximum(100).Value(50).Primary()
                    )
                )
            ).Padding(20)
        );
}
