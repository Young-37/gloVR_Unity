using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dropdowns = transform.GetComponents<Dropdown>();

        foreach(var dropdown in dropdowns){
            dropdown.options.Clear();

            List<string> items = new List<string>();
            items.Add("0단계");
            items.Add("1단계");
            items.Add("2단계");
            items.Add("3단계");

            //Fill Dropdown with items
            foreach(var item in items){
                dropdown.options.Add(new Dropdown.OptionData() {text = item});
            }

            DropdownItemSelected(dropdown);

            dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown);});
        }
    }

    void DropdownItemSelected(Dropdown drd){
        int index = drd.value;
        Debug.Log(drd.options[index].text);

    }

}
