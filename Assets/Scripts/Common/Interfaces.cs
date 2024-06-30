using System.Collections;

public interface IState{
    public void Enter();
    public IEnumerator Update();
    public void Exit();
}