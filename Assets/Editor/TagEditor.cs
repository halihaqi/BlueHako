using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(Tag))]
public class TagEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Tag tag = attribute as Tag;

        //因为attribute会被重新创建
        //所以要先获取属性挂载的物体的值
        //并赋给tag
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

        //修改后直接退出unity不会保存
        //因为自定义编辑器不会让场景知道自己已经做了修改但还没保存。
        //也就是说，需要让编辑器知道自己已经“脏”了，才会使修改可以被保存。
        if (GUI.changed)
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }
}
