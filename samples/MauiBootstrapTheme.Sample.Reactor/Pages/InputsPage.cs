using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class InputsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Input Controls").Class("h1"),
                    Label("Form inputs with Bootstrap styling").Class("lead").Class("text-muted")
                ),

                // Text Inputs
                VStack(spacing: 12,
                    Label("Text Inputs").Class("h4"),
                    VStack(spacing: 4,
                        Label("Default Input").Class("form-label"),
                        Entry().Placeholder("Enter text...").Class("form-control")
                    ),
                    VStack(spacing: 4,
                        Label("Large Input").Class("form-label"),
                        Entry().Placeholder("Large input").Class("form-control").Class("form-control-lg")
                    ),
                    VStack(spacing: 4,
                        Label("Small Input").Class("form-label"),
                        Entry().Placeholder("Small input").Class("form-control").Class("form-control-sm")
                    )
                ),

                // Textarea
                VStack(spacing: 12,
                    Label("Textarea").Class("h4"),
                    VStack(spacing: 4,
                        Label("Default Textarea").Class("form-label"),
                        Editor().Placeholder("Enter multi-line text...").HeightRequest(100).Class("form-control")
                    )
                ),

                // Select
                VStack(spacing: 12,
                    Label("Select").Class("h4"),
                    VStack(spacing: 4,
                        Label("Default Select").Class("form-label"),
                        Picker().Title("Choose an option...").ItemsSource(new[] { "Option 1", "Option 2", "Option 3" }).Class("form-select")
                    )
                ),

                // Date & Time
                VStack(spacing: 12,
                    Label("Date & Time").Class("h4"),
                    Grid("Auto", "*,*",
                        VStack(spacing: 4,
                            Label("Date").Class("form-label"),
                            DatePicker().Class("form-control")
                        ).GridColumn(0),
                        VStack(spacing: 4,
                            Label("Time").Class("form-label"),
                            TimePicker().Class("form-control")
                        ).GridColumn(1)
                    ).ColumnSpacing(20)
                ),

                // Checkboxes & Switches
                VStack(spacing: 12,
                    Label("Checkboxes & Switches").Class("h4"),
                    HStack(spacing: 8,
                        CheckBox().IsChecked(true).Class("form-check-input").Color(GetResource<Color>("Primary")),
                        Label("Default checkbox").Class("form-check-label").VCenter()
                    ),
                    HStack(spacing: 8,
                        CheckBox().Class("form-check-input").Color(GetResource<Color>("Primary")),
                        Label("Unchecked checkbox").Class("form-check-label").VCenter()
                    ),
                    HStack(spacing: 8,
                        Switch().IsToggled(true).Class("form-switch").OnColor(GetResource<Color>("Primary")),
                        Label("Enabled switch").Class("form-check-label").VCenter()
                    ),
                    HStack(spacing: 8,
                        Switch().Class("form-switch").OnColor(GetResource<Color>("Primary")),
                        Label("Disabled switch").Class("form-check-label").VCenter()
                    )
                ),

                // Radio Buttons
                VStack(spacing: 12,
                    Label("Radio Buttons").Class("h4"),
                    VStack(
                        RadioButton().Content("Option 1").IsChecked(true).Class("form-check-input"),
                        RadioButton().Content("Option 2").Class("form-check-input"),
                        RadioButton().Content("Option 3").Class("form-check-input")
                    ).Set(Microsoft.Maui.Controls.RadioButtonGroup.GroupNameProperty, "radioGroup")
                ),

                // Range Slider
                VStack(spacing: 12,
                    Label("Range Slider").Class("h4"),
                    VStack(spacing: 4,
                        Label("Default Range").Class("form-label"),
                        Slider().Minimum(0).Maximum(100).Value(50).Class("form-range")
                            .MinimumTrackColor(GetResource<Color>("Primary"))
                            .ThumbColor(GetResource<Color>("Primary"))
                    )
                )
            ).Padding(20)
        );
}
