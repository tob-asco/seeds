using seeds.Dal.Interfaces;
using seeds.Dal.Model;

namespace seeds.Dal.Services;

public class PresentationService : IPresentationService
{
    private readonly IDalBaseService baseService;

    public PresentationService(
        IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<Presentation?> GetPresentationByIdeaIdAsync(int ideaId)
    {
        string url = $"api/Presentations/{ideaId}";
        return await baseService.GetDalModelAsync<Presentation>(url);
    }

    public async Task PostOrPutPresentationAsync(Presentation presi)
    {
        if (await PostPresentationAsync(presi)) { return; }
        if (await PutPresentationByIdeaIdAsync(presi.Id, presi)) { return; }
        throw new Exception("Neither could we Post, nor Put the specified presentation" +
            $"of Id {presi.Id}.");
    }

    public async Task<bool> PostPresentationAsync(Presentation presi)
    {
        string url = "api/Presentations";
        return await baseService.PostDalModelAsync(url, presi);
    }

    public async Task<bool> PutPresentationByIdeaIdAsync(int ideaId, Presentation presi)
    {
        string url = $"api/Presentations/{ideaId}";
        return await baseService.PutDalModelAsync(url, presi);
    }
}
