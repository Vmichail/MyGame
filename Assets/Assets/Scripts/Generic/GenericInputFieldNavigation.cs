using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericInputFieldNavigation : MonoBehaviour
{
    public Selectable[] order;

    void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;
        if (!current) return;

        int index = System.Array.FindIndex(order, s => s && s.gameObject == current);
        if (index < 0) return;

        int direction = 0;

        // NEXT
        if ((Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = +1;
        }

        // PREVIOUS
        if ((Input.GetKeyDown(KeyCode.Tab) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            || Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = -1;
        }

        if (direction == 0) return;

        Selectable next = FindNext(index, direction);
        if (next != null)
            EventSystem.current.SetSelectedGameObject(next.gameObject);
    }

    Selectable FindNext(int startIndex, int direction)
    {
        int index = startIndex;
        int checkedCount = 0;

        while (checkedCount < order.Length)
        {
            index += direction;

            // wrap
            if (index < 0)
                index = order.Length - 1;
            else if (index >= order.Length)
                index = 0;

            checkedCount++;

            var s = order[index];
            if (s == null) continue;
            if (!s.interactable) continue;
            if (!s.gameObject.activeInHierarchy) continue;

            return s;
        }

        // no valid selectable found
        return null;
    }
}
