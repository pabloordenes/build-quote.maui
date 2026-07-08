using BuildQuote.Domain.Entities;
using BuildQuote.Domain.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static QuestPDF.Helpers.Colors;

namespace BuildQuote.App.ViewModels;

public partial class CreateQuoteViewModel : ObservableObject
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IMaterialRepository _materialRepository;

    // --- DATOS DEL CLIENTE ---
    [ObservableProperty] private string _clientName = string.Empty;
    [ObservableProperty] private string _clientPhone = string.Empty;
    [ObservableProperty] private string _projectAddress = string.Empty;

    // --- GESTIÓN DE ÍTEMS ---
    [ObservableProperty] private ObservableCollection<Material> _availableMaterials = new();
    [ObservableProperty] private Material? _selectedMaterial;
    [ObservableProperty] private decimal _quantity;

    public ObservableCollection<QuoteItem> Items { get; } = new();

    // --- TOTALES EN TIEMPO REAL ---
    [ObservableProperty] private decimal _subtotal;
    [ObservableProperty] private decimal _taxAmount;
    [ObservableProperty] private decimal _total;
    private readonly decimal _taxRate = 0.19m; // 19% IVA

    public CreateQuoteViewModel(IQuoteRepository quoteRepository, IMaterialRepository materialRepository)
    {
        _quoteRepository = quoteRepository;
        _materialRepository = materialRepository;
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        var materials = await _materialRepository.GetAllActiveAsync();
        AvailableMaterials.Clear();
        foreach (var m in materials) AvailableMaterials.Add(m);
    }

    [RelayCommand]
    public void AddItem()
    {
        if (SelectedMaterial == null || Quantity <= 0) return;

        var newItem = new QuoteItem
        {
            MaterialId = SelectedMaterial.Id,
            DescriptionOverride = SelectedMaterial.Name, // Congelamos el nombre
            UnitPrice = SelectedMaterial.UnitCost,       // Congelamos el precio actual
            Quantity = this.Quantity
        };

        Items.Add(newItem);
        RecalculateTotals();

        // Reseteamos el formulario de ingreso
        SelectedMaterial = null;
        Quantity = 0;
    }

    [RelayCommand]
    public void RemoveItem(QuoteItem item)
    {
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotals();
        }
    }

    private void RecalculateTotals()
    {
        // Actualizamos las variables observables para que la UI cambie instantáneamente
        Subtotal = Items.Sum(i => i.TotalPrice);
        TaxAmount = Subtotal * _taxRate;
        Total = Subtotal + TaxAmount;
    }

    [RelayCommand]
    public async Task SaveQuoteAsync()
    {
        if (!Items.Any() || string.IsNullOrWhiteSpace(ClientName))
            return; 

        var quote = new Quote
        {
            ClientName = this.ClientName,
            ClientPhone = this.ClientPhone,
            ProjectAdress = this.ProjectAddress,
            TaxRate = _taxRate,
            Items = this.Items.ToList()
        };

        // Guardamos todo de golpe en SQLite
        await _quoteRepository.AddAsync(quote);

        // Limpiamos la pantalla
        ClientName = string.Empty;
        ClientPhone = string.Empty;
        ProjectAddress = string.Empty;
        Items.Clear();
        RecalculateTotals();
    }
}