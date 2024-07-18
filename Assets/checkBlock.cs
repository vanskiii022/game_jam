using UnityEngine;

public class CameraLineOfSight : MonoBehaviour
{
    public Transform target; // 主角的Transform
    public LayerMask obstructionLayer; // 用于检测遮挡的LayerMask
    private GameObject currentObstruction = null; // 当前遮挡物体

    private void Update()
    {
        CheckLineOfSight();
    }

    private void CheckLineOfSight()
    {
        target = PlayerManager.Instance.transform;
        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        Ray ray = new Ray(transform.position, directionToTarget);
        RaycastHit hit;

        // 可视化射线
        Debug.DrawLine(transform.position, target.position, Color.red);

        // 检查射线是否击中了某个物体
        if (Physics.Raycast(ray, out hit, distanceToTarget, obstructionLayer))
        {
            if (hit.collider.CompareTag("roof"))
            {
                // 如果当前遮挡物体不是已记录的遮挡物体，则隐藏新的遮挡物体
                if (currentObstruction != hit.collider.gameObject)
                {
                    // 显示之前的遮挡物体（如果有）
                    if (currentObstruction != null)
                    {
                        ShowObstruction();
                    }
                    // 记录并隐藏当前的遮挡物体
                    currentObstruction = hit.collider.gameObject;
                    HideObstruction(currentObstruction);
                }
            }
            else
            {
                // 如果射线击中的不是roof标签的物体，则确保之前隐藏的物体现在可见
                if (currentObstruction != null)
                {
                    ShowObstruction();
                    currentObstruction = null;
                }
            }
        }
        else
        {
            // 如果射线没有击中任何物体，则确保之前隐藏的物体现在可见
            if (currentObstruction != null)
            {
                ShowObstruction();
                currentObstruction = null;
            }
        }
    }

    private void HideObstruction(GameObject obstruction)
    {
        // 获取并隐藏遮挡物体的MeshRenderer组件，使其在视觉上不可见
        MeshRenderer meshRenderer = obstruction.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    private void ShowObstruction()
    {
        // 显示之前隐藏的遮挡物体的MeshRenderer组件
        if (currentObstruction != null)
        {
            MeshRenderer meshRenderer = currentObstruction.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
            currentObstruction = null; // 清除引用
        }
    }

}
