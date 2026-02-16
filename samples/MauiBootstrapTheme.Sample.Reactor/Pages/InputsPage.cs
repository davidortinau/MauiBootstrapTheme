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
                        Label("Default Input").TextStyle(BootstrapTextStyle.Small),
                        Entry().Placeholder("Enter text...").BootstrapHeight()
                    ),
                    VStack(spacing: 4,
                        Label("Large Input").TextStyle(BootstrapTextStyle.Small),
                        Entry().Placeholder("Large input").Large().HeightRequest(48)
                    ),
                    VStack(spacing: 4,
                        Label("Small Input").TextStyle(BootstrapTextStyle.Small),
                        Entry().Placeholder("Small input").Small().HeightRequest(31)
                    )
                ),

                // Textarea
                VStack(spacing: 12,
                    Label("Textarea").H4(),
                    VStack(spacing: 4,
                        Label("Default Textarea").TextStyle(BootstrapTextStyle.Small),
                        Editor().Placeholder("Enter multi-line text...").HeightRequest(100)
                    )
                ),

                // Select
                VStack(spacing: 12,
                    Label("Select").H4(),
                    VStack(spacing: 4,
                        Label("Default Select").TextStyle(BootstrapTextStyle.Small),
                        Picker().Title("Choose an option...").ItemsSource(new[] { "Option 1", "Option 2", "Option 3" })
                    )
                ),

                // Date & Time
                VStack(spacing: 12,
                    Label("Date & Time").H4(),
                    HStack(spacing: 20,
                        VStack(spacing: 4,
                            Label("Date").TextStyle(BootstrapTextStyle.Small),
                            DatePicker()
                        ).HFill(),
                        VStack(spacing: 4,
                            Label("Time").TextStyle(BootstrapTextStyle.Small),
                            TimePicker()
                        ).HFill()
                    )
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

                // Range Slider
                VStack(spacing: 12,
                    Label("Range Slider").H4(),
                    VStack(spacing: 4,
                        Label("Default Range").TextStyle(BootstrapTextStyle.Small),
                        Slider().Minimum(0).Maximum(100).Value(50).Primary()
                    )
                )
            ).Padding(20)
        );
}
