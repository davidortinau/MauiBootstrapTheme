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
                        Label("Email address").Class("form-label").Class("small"),
                        Entry()
                            .Placeholder("name@example.com")
                            .Keyboard(Keyboard.Email)
                            .Text(State.Email)
                            .OnTextChanged(text => SetState(s => s.Email = text))
                            .Class("form-control"),
                        Label("We'll never share your email with anyone else.").Class("form-text")
                    ),

                    VStack(spacing: 4,
                        Label("Password").Class("form-label").Class("small"),
                        Entry()
                            .IsPassword(true)
                            .Text(State.Password)
                            .OnTextChanged(text => SetState(s => s.Password = text))
                            .Class("form-control")
                    ),

                    HStack(spacing: 8,
                        CheckBox()
                            .IsChecked(State.IsChecked)
                            .OnCheckedChanged(chk => SetState(s => s.IsChecked = chk))
                            .Class("form-check-input"),
                        Label("Check me out").Class("form-check-label").VCenter()
                    ),

                    Button("Submit").Class("btn").Class("btn-primary").HorizontalOptions(LayoutOptions.Start)
                ),

                // Sizing
                VStack(spacing: 12,
                    Label("Sizing").H4(),

                    VStack(spacing: 4,
                        Label("Large input").Class("form-label").Class("small"),
                        Entry().Placeholder(".form-control-lg").Class("form-control").Class("form-control-lg")
                    ),

                    VStack(spacing: 4,
                        Label("Default input").Class("form-label").Class("small"),
                        Entry().Placeholder(".form-control").Class("form-control")
                    ),

                    VStack(spacing: 4,
                        Label("Small select").Class("form-label").Class("small"),
                        Picker()
                            .Title(".form-select-sm")
                            .ItemsSource(new[] { "Option 1", "Option 2" })
                            .Class("form-select").Class("form-select-sm")
                    )
                ),

                // Disabled forms
                VStack(spacing: 12,
                    Label("Disabled forms").H4(),
                    Label("Disabled fieldset example").Class("small").Muted(),

                    VStack(spacing: 4,
                        Label("Disabled input").Class("form-label").Class("small"),
                        Entry().Placeholder("Disabled input").Class("form-control")
                    ),

                    VStack(spacing: 4,
                        Label("Disabled select menu").Class("form-label").Class("small"),
                        Picker()
                            .Title("Disabled select")
                            .ItemsSource(new[] { "Disabled select" })
                            .Class("form-select")
                    ),

                    Button("Submit").Class("btn").Class("btn-primary").HorizontalOptions(LayoutOptions.Start)
                ).IsEnabled(false),

                // Form layout
                VStack(spacing: 12,
                    Label("Form layout").H4(),

                    Grid("Auto,Auto,Auto", "*,*",
                        VStack(spacing: 4,
                            Label("First name").Class("form-label").Class("small"),
                            Entry().Class("form-control")
                        ).GridRow(0).GridColumn(0),

                        VStack(spacing: 4,
                            Label("Last name").Class("form-label").Class("small"),
                            Entry().Class("form-control")
                        ).GridRow(0).GridColumn(1),

                        VStack(spacing: 4,
                            Label("Email").Class("form-label").Class("small"),
                            Entry().Keyboard(Keyboard.Email).Class("form-control")
                        ).GridRow(1).GridColumnSpan(2),

                        VStack(spacing: 4,
                            Label("Message").Class("form-label").Class("small"),
                            Editor().HeightRequest(100).Class("form-control")
                        ).GridRow(2).GridColumnSpan(2)
                    ).ColumnSpacing(12).RowSpacing(8)
                )
            ).Padding(20)
        );
}
