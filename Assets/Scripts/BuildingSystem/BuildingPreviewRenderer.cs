using System.Collections.Generic;
using UnityEngine;

public class BuildingPreviewRenderer : MonoBehaviour
{
    [SerializeField]
    private Material validMateiral;
    [SerializeField]
    private Material invalidMateiral;

    private GameObject currentPreview;

    private List<Renderer> previewRenderers = new List<Renderer>();

    public void CreatePreview(GameObject previewPF)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        previewRenderers.Clear();

        if (previewPF == null)
        {
            return;
        }

        // 미리보기 오브젝트 생성
        currentPreview = Instantiate(previewPF);
        // 미리보기 오브젝트의 렌더러들을 리스트에 저장
        previewRenderers.AddRange(currentPreview.GetComponentsInChildren<Renderer>());
        // 미리보기는 처음에는 꺼둠
        currentPreview.SetActive(false);
    }

    public void ShowPreview(bool show)
    {
        if (currentPreview == null)
        {
            return;
        }

        currentPreview.SetActive(show);
    }

    public void UpdateTransform(Vector3 position, Quaternion rotation)
    {
        if (currentPreview != null)
        {
            currentPreview.transform.position = position;
            currentPreview.transform.rotation = rotation;
        }
    }

    public void SetValidityColor(bool isValid)
    {
        if (currentPreview == null) return;

        Material targetMaterial = isValid ? validMateiral : invalidMateiral;

        foreach (Renderer r in previewRenderers)
        {
            r.material = targetMaterial;
        }
    }
}
