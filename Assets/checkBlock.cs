using UnityEngine;

public class CameraLineOfSight : MonoBehaviour
{
    public Transform target; // ���ǵ�Transform
    public LayerMask obstructionLayer; // ���ڼ���ڵ���LayerMask
    private GameObject currentObstruction = null; // ��ǰ�ڵ�����

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

        // ���ӻ�����
        Debug.DrawLine(transform.position, target.position, Color.red);

        // ��������Ƿ������ĳ������
        if (Physics.Raycast(ray, out hit, distanceToTarget, obstructionLayer))
        {
            if (hit.collider.CompareTag("roof"))
            {
                // �����ǰ�ڵ����岻���Ѽ�¼���ڵ����壬�������µ��ڵ�����
                if (currentObstruction != hit.collider.gameObject)
                {
                    // ��ʾ֮ǰ���ڵ����壨����У�
                    if (currentObstruction != null)
                    {
                        ShowObstruction();
                    }
                    // ��¼�����ص�ǰ���ڵ�����
                    currentObstruction = hit.collider.gameObject;
                    HideObstruction(currentObstruction);
                }
            }
            else
            {
                // ������߻��еĲ���roof��ǩ�����壬��ȷ��֮ǰ���ص��������ڿɼ�
                if (currentObstruction != null)
                {
                    ShowObstruction();
                    currentObstruction = null;
                }
            }
        }
        else
        {
            // �������û�л����κ����壬��ȷ��֮ǰ���ص��������ڿɼ�
            if (currentObstruction != null)
            {
                ShowObstruction();
                currentObstruction = null;
            }
        }
    }

    private void HideObstruction(GameObject obstruction)
    {
        // ��ȡ�������ڵ������MeshRenderer�����ʹ�����Ӿ��ϲ��ɼ�
        MeshRenderer meshRenderer = obstruction.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    private void ShowObstruction()
    {
        // ��ʾ֮ǰ���ص��ڵ������MeshRenderer���
        if (currentObstruction != null)
        {
            MeshRenderer meshRenderer = currentObstruction.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
            currentObstruction = null; // �������
        }
    }

}
