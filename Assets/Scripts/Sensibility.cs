using UnityEngine;
using UnityEngine.UI;

public class Sensibility : MonoBehaviour
{

    void Start()
    {
        Slider slider = GetComponent<Slider>();
        if (slider != null) slider.value = PlayerPrefs.GetFloat ("Sensibilidade", 1f);
    }

    public void Salvar(float Valor)
    {
        PlayerPrefs.SetFloat ("Sensibilidade", Valor);
    }
}
    