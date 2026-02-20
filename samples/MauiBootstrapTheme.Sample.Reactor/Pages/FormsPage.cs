using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

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
                    Label("Forms").Class(Bs.H1),
                    Label("Canonical Bootstrap 5.3 form patterns for parity comparison.").Class(Bs.Lead).Class(Bs.TextMuted)
                ),

                // Overview
                VStack(spacing: 12,
                    Label("Overview").Class(Bs.H4),

                    VStack(spacing: 4,
                        Label("Email address").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry()
                            .Placeholder("name@example.com")
                            .Keyboard(Keyboard.Email)
                            .Text(State.Email)
                            .OnTextChanged(text => SetState(s => s.Email = text))
                            .Class(Bs.FormControl),
                        Label("We'll never share your email with anyone else.").Class(Bs.FormText)
                    ),

                    VStack(spacing: 4,
                        Label("Password").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry()
                            .IsPassword(true)
                            .Text(State.Password)
                            .OnTextChanged(text => SetState(s => s.Password = text))
                            .Class(Bs.FormControl)
                    ),

                    HStack(spacing: 8,
                        CheckBox()
                            .IsChecked(State.IsChecked)
                            .OnCheckedChanged(chk => SetState(s => s.IsChecked = chk))
                            .Class(Bs.FormCheckInput),
                        Label("Check me out").Class(Bs.FormCheckLabel).VCenter()
                    ),

                    Button("Submit").Class(Bs.BtnPrimary).HorizontalOptions(LayoutOptions.Start)
                ),

                // Sizing
                VStack(spacing: 12,
                    Label("Sizing").Class(Bs.H4),

                    VStack(spacing: 4,
                        Label("Large input").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry().Placeholder(".form-control-lg").Class(Bs.FormControl).Class(Bs.FormControlLg)
                    ),

                    VStack(spacing: 4,
                        Label("Default input").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry().Placeholder(".form-control").Class(Bs.FormControl)
                    ),

                    VStack(spacing: 4,
                        Label("Small select").Class(Bs.FormLabel).Class(Bs.Small),
                        Picker()
                            .Title(".form-select-sm")
                            .ItemsSource(new[] { "Option 1", "Option 2" })
                            .Class(Bs.FormSelect).Class(Bs.FormSelectSm)
                    )
                ),

                // Disabled forms
                VStack(spacing: 12,
                    Label("Disabled forms").Class(Bs.H4),
                    Label("Disabled fieldset example").Class(Bs.Small).Class(Bs.TextMuted),

                    VStack(spacing: 4,
                        Label("Disabled input").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry().Placeholder("Disabled input").Class(Bs.FormControl)
                    ),

                    VStack(spacing: 4,
                        Label("Disabled select menu").Class(Bs.FormLabel).Class(Bs.Small),
                        Picker()
                            .Title("Disabled select")
                            .ItemsSource(new[] { "Disabled select" })
                            .Class(Bs.FormSelect)
                    ),

                    Button("Submit").Class(Bs.BtnPrimary).HorizontalOptions(LayoutOptions.Start)
                ).IsEnabled(false),

                // Form layout
                VStack(spacing: 12,
                    Label("Form layout").Class(Bs.H4),

                    Grid("Auto,Auto,Auto", "*,*",
                        VStack(spacing: 4,
                            Label("First name").Class(Bs.FormLabel).Class(Bs.Small),
                            Entry().Class(Bs.FormControl)
                        ).GridRow(0).GridColumn(0),

                        VStack(spacing: 4,
                            Label("Last name").Class(Bs.FormLabel).Class(Bs.Small),
                            Entry().Class(Bs.FormControl)
                        ).GridRow(0).GridColumn(1),

                        VStack(spacing: 4,
                            Label("Email").Class(Bs.FormLabel).Class(Bs.Small),
                            Entry().Keyboard(Keyboard.Email).Class(Bs.FormControl)
                        ).GridRow(1).GridColumnSpan(2),

                        VStack(spacing: 4,
                            Label("Message").Class(Bs.FormLabel).Class(Bs.Small),
                            Editor().HeightRequest(100).Class(Bs.FormControl)
                        ).GridRow(2).GridColumnSpan(2)
                    ).ColumnSpacing(12).RowSpacing(8)
                )
            ).Padding(20)
        );
}
