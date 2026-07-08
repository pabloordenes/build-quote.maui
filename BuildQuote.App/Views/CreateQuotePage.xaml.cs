using BuildQuote.App.ViewModels;

namespace BuildQuote.App.Views;

public partial class CreateQuotePage : ContentPage
{
    private readonly CreateQuoteViewModel _viewModel;

    public CreateQuotePage(CreateQuoteViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync(); // Carga el inventario al abrir la pantalla
    }
}