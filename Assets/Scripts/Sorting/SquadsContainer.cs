using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadsContainer : MonoBehaviour
{
    [SerializeField] private int _maxUnits = 4;
    [SerializeField] private List<Squad> _currentSquads;
    [SerializeField] private float _unitZStep = 1f;

    [Header("Colors")]
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _completedColor;
    [SerializeField] private Color _badColor;

    private Material _material;
    private ContainerBorders _borders;

    private Coroutine _changeColorJob;

    public bool IsFree => _currentSquads.Count == 0 ? true : false;
    private Squad LastSquad => _currentSquads[_currentSquads.Count - 1];

    private int TotalUnits
    {
        get
        {
            int value = 0;
            foreach (Squad squad in _currentSquads)
                value += squad.UnitsCount;
            return value;
        }
    }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _borders = GetComponent<ContainerBorders>();

        _borders.UpdateBorders(TotalUnits);
    }

    public bool TryMoveSquad(SquadsContainer fromContainer) // не нравится что возвращает bool !isNewSelect
    {
        Squad fromSquad = fromContainer.LastSquad;

        if (IsFree)
        {
            MoveSquadUnits(fromSquad, fromSquad);
            AddSquad(fromSquad);

            fromContainer.RemoveSquad(fromSquad);
        }
        else
        {
            Squad targetSquad = LastSquad;

            if (fromSquad.UnitsType == targetSquad.UnitsType)
            {
                if (IsEnoughCapacity(fromSquad))
                {
                    MoveSquadUnits(fromSquad, targetSquad);
                    targetSquad.Combine(fromSquad);

                    fromSquad.ClearSquad();
                    fromContainer.RemoveSquad(fromSquad);

                    Debug.Log("отряд из " + fromContainer.name + " совмещаем с " + name);
                }
                else
                {
                    Debug.Log(name + " не может вместить столько юнитов");
                    FocusBad();
                }
            }
            else
            {
                Debug.Log("отряды контейнеров не сопадают, выбрать новый контейнер");
                FocusBad();
            }
        }

        if (_currentSquads.Count == 1 && _currentSquads[0].UnitsCount == _maxUnits)
            ChangeColor(_completedColor);

        return true;
    }

    private void MoveSquadUnits(Squad fromSquad, Squad targetSquad)
    {
        Vector3 targetSquadPosition = transform.position - new Vector3(0, 0, 1.5f) + new Vector3(0, 0, _unitZStep) * TotalUnits;

        fromSquad.MoveSquad(targetSquad, targetSquadPosition, new Vector2(0, 1));

        UnitsAudio.Instance.Agreement();
    }

    private void AddSquad(Squad squad)
    {
        _currentSquads.Add(squad);
    }

    private void RemoveSquad(Squad squad)
    {
        _currentSquads.Remove(squad);
    }

    private bool IsEnoughCapacity(Squad fromSquad)
    {
        return TotalUnits + fromSquad.UnitsCount <= _maxUnits;
    }

    public void FocusOn()
    {
        LastSquad.SelectSquad();
        _borders.UpdateBorders(TotalUnits);

        ChangeColor(_selectedColor);

        UnitsAudio.Instance.Select();
    }

    public void FocusOff()
    {
        if (!IsFree)
            LastSquad.UnselectSquad();

        _borders.UpdateBorders(TotalUnits);
        //_material.SetColor("_Color", _baseColor);
        ChangeColor(_baseColor);
    }

    public void FocusBad()
    {
        StartCoroutine(FlashingColor(_badColor));

        UnitsAudio.Instance.Disagreement();
    }

    private void ChangeColor(Color targetColor)
    {
        if (_changeColorJob != null)
            StopCoroutine(_changeColorJob);

        _changeColorJob = StartCoroutine(ChangingColor(targetColor));
    }

    private IEnumerator FlashingColor(Color targetColor)
    {
        if (_changeColorJob != null)
            StopCoroutine(_changeColorJob);

        _changeColorJob = StartCoroutine(ChangingColor(targetColor));
        yield return _changeColorJob;
        _changeColorJob = StartCoroutine(ChangingColor(_baseColor));
    }

    private IEnumerator ChangingColor(Color targetColor)
    {
        Color color = _material.color;

        float time = 0;

        while (time < 1)
        {
            _material.color = Color.Lerp(color, targetColor, time);
            time += Time.deltaTime / 0.3f;
            yield return null;
        }

        _changeColorJob = null;
    }
}
