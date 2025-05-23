public interface IPetActivity
{
    string Name { get; }
    void StartActivity(PetStateManager pet);
    void UpdateActivity(PetStateManager pet);
    void EndActivity(PetStateManager pet);
}
