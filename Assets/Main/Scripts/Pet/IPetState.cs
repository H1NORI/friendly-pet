
public interface IPetState
{
    void EnterState(PetStateManager pet);
    void UpdateState(PetStateManager pet);
    void ExitState(PetStateManager pet);
}
