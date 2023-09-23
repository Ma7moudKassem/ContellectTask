namespace ContellectTask.WebUI.Components;

public partial class Pagination
{
    [Parameter] public MetaData MetaData { get; set; }
    [Parameter] public int Spread { get; set; }
    [Parameter] public EventCallback<int> OnChange { get; set; }

    private List<PagingLink> links;

    protected override async Task OnParametersSetAsync()
    {
        await CreatePaginationLinks();

        await base.OnParametersSetAsync();
    }

    private async Task CreatePaginationLinks()
    {
        links = new()
        {
            new PagingLink(MetaData.CurrentPage - 1, MetaData.HasPrevious, Resource.Previous)
        };

        for (int i = 1; i <= MetaData.TotalPages; i++)
        {
            if (i >= MetaData.CurrentPage - Spread && i <= MetaData.CurrentPage + Spread)
                links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaData.CurrentPage == i });
        }

        links.Add(new PagingLink(MetaData.CurrentPage + 1, MetaData.HasNext, Resource.Next));

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnSelectedPage(PagingLink link)
    {
        if (link.Page == MetaData.CurrentPage || !link.Enabled)
            return;

        MetaData.CurrentPage = link.Page;

        await OnChange.InvokeAsync(link.Page);

        await InvokeAsync(StateHasChanged);
    }
}