public interface IPetActivity
{
    string Name { get; }
    int minCoins { get; set; }
    int maxCoins { get; set; }
    void StartActivity(PetStateManager pet);
    void UpdateActivity(PetStateManager pet);
    void EndActivity(PetStateManager pet);
}
