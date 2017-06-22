using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PresentationControls;

namespace AccessControlSystem.Lib
{
    class CheckBoxComboBoxOP
    {
        /// <summary>
        /// 通过字符串构建CheckBoxComboBox
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public CheckBoxComboBox createCheckBoxComboBox(string str)
        {
            CheckBoxComboBox checkBoxComboBox = new CheckBoxComboBox();

            string[] s = str.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            for (int idx = 0; idx < s.Length; idx++)
            {
                checkBoxComboBox.Items.Add(s[idx]);
            }
            return checkBoxComboBox;
        }
        /// <summary>
        /// 勾选指定项
        /// </summary>
        /// <param name="checkBoxComboBox"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        static public CheckBoxComboBox checkItem(CheckBoxComboBox checkBoxComboBox, string str)
        {
            string[] s = str.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            for (int idx = 0; idx < checkBoxComboBox.Items.Count; idx++) /* 先取消所有勾选 */
            {
                checkBoxComboBox.CheckBoxItems[idx].Checked = false; ;
            }
            for (int idx = 0; idx < s.Length; idx++)                     /* 然后勾选指定项 */
            {
                checkBoxComboBox.CheckBoxItems[s[idx]].Checked = true; ;
            }
            return checkBoxComboBox;
        }
    }
}
