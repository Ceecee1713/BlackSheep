using UnityEngine;

public class MovingButton : MonoBehaviour
{
    [SerializeField]
    private Transform [] transformsToMoveAt = new Transform [3];

    private Vector3 _originalPosition;
    private int _counter = -1;

    void Start()
    {
      _originalPosition = this.transform.position;  
    }

    public void OnMoveButtonClick()
    {
        _counter++;

        if(_counter == transformsToMoveAt.Length)
            this.transform.position = _originalPosition; 

        if(_counter > transformsToMoveAt.Length)
            this.gameObject.SetActive(false);

        if(_counter < transformsToMoveAt.Length)
            this.transform.position = transformsToMoveAt[_counter].position;
    }
}
