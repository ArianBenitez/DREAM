using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// Va en Canvas_Web y sustituye a su Graphic Raycaster.
// La web se ve curvada en Pantalla_Web, así que cada botón aparece deplazado respecto a su posición real. Aquí se corrige la 
// posición del ratón antes de comprobar qué botón tiene debajo.
public class RaycasterPantalla : GraphicRaycaster
{
    [SerializeField] RawImage pantalla; // Pantalla_Web, en el Canvas principal
    [SerializeField] Volume volumen; // Volume_Web, de donde se leen los valores de la curva

    public override void Raycast(PointerEventData datos, List<RaycastResult> resultados)
    {
        Vector2 original = datos.position;

        // Posición del ratón dentro de Pantalla_Web, de 0 a 1 en cada eje.
        // Fuera de la pantalla no hay botones
        RectTransform rt = pantalla.rectTransform;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, original, null, out Vector2 local)) return;
        Vector2 uv = (local - rt.rect.min) / rt.rect.size;
        if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1) return;

        // Se aplica la curva y se pasa a píxeles de RT_Web, que es donde está la web de verdad.
        // Después se devuelve la posición original para no afectar al resto del sistema
        uv = Curvar(uv);
        datos.position = new Vector2(uv.x * pantalla.texture.width, uv.y * pantalla.texture.height);
        base.Raycast(datos, resultados);
        datos.position = original;
    } 

    // Repite la cuenta del shader de Lens Distortion de URP: dado un punto de la imagen curvada, devuelve el punto de la 
    // textura que se dibuja en él
    Vector2 Curvar(Vector2 uv)
    {
        if (!volumen.profile.TryGet(out LensDistortion lente) || !lente.active) return uv;

        float intensidad = lente.intensity.value;
        float theta = Mathf.Deg2Rad * Mathf.Min(160f, 1.6f * Mathf.Max(Mathf.Abs(intensidad * 100f), 1f));
        float sigma = 2f * Mathf.Tan(theta * 0.5f);
        Vector2 centro = lente.center.value * 2f - Vector2.one;
        Vector2 eje = new Vector2(Mathf.Max(lente.xMultiplier.value, 1e-4f), Mathf.Max(lente.yMultiplier.value, 1e-4f));
        Vector2 medio = new Vector2(0.5f, 0.5f);

        uv = (uv - medio) / lente.scale.value + medio;
        Vector2 r = Vector2.Scale(eje, uv - medio - centro);
        float ru = r.magnitude;
        if (ru < 1e-5f) return uv; // el centro no se mueve

        // intensidad positiva curva la imagen hacia fuera u negativa la hunde hacia dentro
        if (intensidad > 0) ru = Mathf.Tan(ru * theta) / ((ru * sigma));
        else ru = Mathf.Atan(ru * sigma) / (ru * theta);
        return uv + r * (ru - 1f);
    }
}
