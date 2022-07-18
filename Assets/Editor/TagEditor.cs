using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(Tag))]
public class TagEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Tag tag = attribute as Tag;

        //��Ϊattribute�ᱻ���´���
        //����Ҫ�Ȼ�ȡ���Թ��ص������ֵ
        //������tag
        string frontStr = (string)fieldInfo.GetValue(property.serializedObject.targetObject);
        for (int i = 0; i < InternalEditorUtility.tags.Length; i++)
        {
            if(InternalEditorUtility.tags[i] == frontStr)
            {
                tag.Selected = i;
                tag.Name = frontStr;
            }
        }

        tag.Selected = EditorGUI.Popup(position, label.text, tag.Selected, InternalEditorUtility.tags);
        tag.Name = InternalEditorUtility.tags[tag.Selected];
        fieldInfo.SetValue(property.serializedObject.targetObject, tag.Name);

        //�޸ĺ�ֱ���˳�unity���ᱣ��
        //��Ϊ�Զ���༭�������ó���֪���Լ��Ѿ������޸ĵ���û���档
        //Ҳ����˵����Ҫ�ñ༭��֪���Լ��Ѿ����ࡱ�ˣ��Ż�ʹ�޸Ŀ��Ա����档
        if (GUI.changed)
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }
}
