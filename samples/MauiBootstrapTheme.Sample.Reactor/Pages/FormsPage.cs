using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class FormsPageState
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsChecked { get; set; }
}

class FormsPage : BasePage<FormsPageState>
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Header
                VStack(spacing: 4,
                    Label("Forms").H1(),
                    Label("Canonical Bootstrap 5.3 form patterns for parity comparison.").Lead().Muted()
                ),

                // Overview
                VStack(spacing: 12,
                    Label("Overview").H4(),

                    VStack(spacing: 4,
                        Label("Email address").TextStyle(BootstrapTextStyle.Small),
                        Entry()
                            .Placeholder("name@example.com")
                            .Keyboard(Keyboard.Email)
                            .Text(State.Email)
                            .OnTextChanged(text => SetState(s => s.Email = text)),
                        Label("We'll never share your email with anyone else.").Muted().TextStyle(BootstrapTextStyle.Small)
                    ),

                    VStack(spacing: 4,
                        Label("Password").TextStyle(BootstrapTextStyle.Small),
                        Entry()
                            .IsPassword(true)
                            .Text(State.Password)
                            .OnTextChanged(text => SetState(s => s.Password = text))
                    ),

                    HStack(spacing: 8,
                        CheckBox()
                            .IsChecked(State.IsChecked)
                            .OnCheckedChanged(chk => SetState(s => s.IsChecked = chk))
                            .Primary(),
                        Label("Check me out").VCenter()
                    ),

                    Button("Submit").Primary().HorizontalOptions(LayoutOptions.Start)
                ),

                // Sizing
                VStack(spacing: 12,
                    Label("Sizing").H4(),

                    VStack(spacing: 4,
                        Label("Large input").TextStyle(BootstrapTextStyle.Small),
                        Entry().Placeholder(".form-control-lg").Large()
                    ),

                    VStack(spacing: 4,
                        Label("Default input").TextStyle(BootstrapTextStyle.Small),
                        Entry().Placeholder(".form-control")
                    ),

                    VStack(spacing: 4,
                        Label("Small select").TextStyle(BootstrapTextStyle.Small),
                        Picker()
                            .Title(".form-select-sm")
                            .ItemsSource(new[] { "Option 1", "Option 2" })
                            .Small()
                    )
                ),

                // Disabled forms
                VStack(spacing: 12,
                    Label("Disabled forms").H4(),
                    Label("Disabled fieldset example").TextStyle(BootstrapTextStyle.Small).Muted(),

                    VStack(spacing: 4,
                        Label("Disabled input").TextStyle(BootstrapTextStyle.Small),
                        Entry().Placeholder("Disabled input")
                    ),

                    VStack(spacing: 4,
                        Label("Disabled select menu").TextStyle(BootstrapTextStyle.Small),
                        Picker()
                            .Title("Disabled select")
                            .ItemsSource(new[] { "Disabled select" })
                    ),

                    Button("Submit").Primary().HorizontalOptions(LayoutOptions.Start)
                ).IsEnabled(false),

                // Form layout
                VStack(spacing: 12,
                    Label("Form layout").H4(),

                    Grid("Auto,Auto,Auto", "*,*",
                        VStack(spacing: 4,
                            Label("First name").TextStyle(BootstrapTextStyle.Small),
                            Entry()
                        ).GridRow(0).GridColumn(0),

                        VStack(spacing: 4,
                            Label("Last name").TextStyle(BootstrapTextStyle.Small),
                            Entry()
                        ).GridRow(0).GridColumn(1),

                        VStack(spacing: 4,
                            Label("Email").TextStyle(BootstrapTextStyle.Small),
                            Entry().Keyboard(Keyboard.Email)
                        ).GridRow(1).GridColumnSpan(2),

                        VStack(spacing: 4,
                            Label("Message").TextStyle(BootstrapTextStyle.Small),
                            Editor().HeightRequest(100)
                        ).GridRow(2).GridColumnSpan(2)
                    ).ColumnSpacing(12).RowSpacing(8)
                )
            ).Padding(20)
        );
}
