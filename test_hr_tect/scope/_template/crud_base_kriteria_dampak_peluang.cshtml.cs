using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace appglobal
{
    public class crud_base_kriteria_dampak_peluang
    {

        public static string get_summernote(string label, string name, string value, bool cannot_null = false, bool script = true)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
            string script_text = script == false ? "" : "<script> $('#" + name + @"').summernote({
                 height: 300,
                 minheight: null,
                 maxheight: null,
                 focus: true
               }); //ckeditor.replace('" + name + @"'); </script>";

            string input_group = @"<h4 class='card-title'>" + label + @"</h4>
               <div class='form-group'><textarea rows='3' class='form-control " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"'>" + value + "</textarea></div>" + script_text;
            return input_group;
        }

        public static string get_input_group(string label, string name, string value, string type = "text", bool cannot_null = false)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";

            string input_group = @"<h4 class='card-title'>" + label + @"</h4>
                                <div class='form-group'>
                                    <input type='" + type + "' class='form-control " + cannot_null_value + @"' id='" + name + @"' placeholder='" + label + @"' name = '" + name + @"' value = '" + value + @"'>
                                </div>";
            return input_group;
        }


        /// <summary>
        /// get input group select2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="label"></param>
        /// <param name="disabled"></param>
        /// <param name="cannot_null"></param>
        /// <param name="select_list"></param>
        /// <param name="default_value"></param>
        /// <param name="multiple"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static string get_select2_group(string label, string name, bool cannot_null = true, dynamic select_list = null, string default_value = "-1", bool disabled = false, bool multiple = false, bool script = true)
        {

            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
            string multiple_select = multiple == false ? "" : "multiple='multiple'";
            string name_input = multiple == false ? name : name + "[]";
            string scripts = script == true ? "<script type='text/javascript'>$(document).ready(function() {$('#" + name + @"').select2({placeholder: 'Pilih " + label + @"',allowClear: true, width: '100%'});$('#" + name + @"').on('change', function () {$('#" + name + @"_area').val($(this).val())})});</script>" : "";
            string disabled_string = disabled == false ? "" : "disabled";
            string select = "<select  name='" + name_input + "' id='" + name + "' placeholder='" + label + @"' class='form-control " + cannot_null_value + "' " + disabled_string + " " + multiple_select + @">";
            select += "<option value = ''>Pilih " + label + "</option>";
            if (select_list != null)
            {
                foreach (var select_list_data in select_list)
                {
                    //logic to parsing anonymous list with propertyInfo looping
                    Type t = select_list_data.GetType();
                    PropertyInfo[] pi = t.GetProperties();

                    //define what property to be assigned
                    string select_value = pi[0].GetValue(select_list_data) + "";
                    string select_display = pi[1].GetValue(select_list_data);

                    //create select options
                    select = select + "<option value='" + select_value + "' ";
                    // AppGlobal.console_log("select_value", select_value);
                    //AppGlobal.console_log("default_value", default_value);
                    if (select_value + "" == default_value)
                    {
                        select = select + "selected";
                    }
                    //AppGlobal.console_log("select", select);
                    select = select + ">" + select_display + "</option>";
                }
            }
            select = select + "</select>";

            string input_group = @"
                <div class='form-group'>
                        <label>" + label + @"</label>
                        " + select + @"
                    </div>" + scripts;
            return input_group;
        }

        /// <summary>
        /// Generate edit button
        /// </summary>
        /// <param name="id">id that being used in generating controls</param>
        /// <returns></returns>
        public static string get_custom_button(string label, string name, string classes= "btn btn-primary",string events="", string icon = "",string style="")
        {
            string edit_button = @"<button type='button' id='" + name + @"' class='btn-fw " + classes + @"' onclick='"+ events + @"' style='"+ style + @"'>"+ label + @"</button> ";
            return edit_button;
        }

        public static string get_file_upload(string label, string name, bool cannot_null = false)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string input_group = @"
                    <h4 class='card-title'>" + label + @"</h4>
                    <div class='custom-file'>
                        <input type='file' class='custom-file-input " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"'>
        			    <input type='hidden' id='hidden_" + name + @"' name='hidden_" + name + @"'>
        			    <input type='hidden' id='name_" + name + @"' name='name_" + name + @"'>
                        <label class='custom-file-label' for=''" + name + @"''>Choose file</label>
                    </div>
            <script>
        	    $(function () {
                    $('.custom-file-input').on('change', function () {
                        var fileName = $(this).val().split('\\').pop();
                        $(this).siblings('.custom-file-label').addClass('selected').html(fileName);
        		        $('#name_" + name + @"').val(fileName);

                        if (this.files && this.files[0]) {
        				    var reader = new FileReader();
        				    reader.onload = image;
        				    reader.readAsDataURL(this.files[0]);
        			    }
                    });
        	    });

        	    function image(e) {
        		    $('#hidden_" + name + @"').val(e.target.result);
        	    };
            </script>";
            return input_group;
        }

        /// <summary>
        /// Generate delete button
        /// </summary>
        /// <param name="id">Id that being used to delete data</param>
        /// <returns></returns>
        public static string get_modal_content(string label, string name)
        {
            string html = @"<div class='modal fade' id='"+name+@"' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'>
                        <div class='modal-dialog modal-lg' role='document'>
                            <div class='modal-content'>
                                <div class='modal-header'>
                                    <h5 class='modal-title' id='exampleModalLabel'>"+label+@"</h5>
                                    <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                        <span aria-hidden='true'>&times;</span>
                                    </button>
                                </div>
                                <div class='modal-body' id='modal_content_"+name+@"'>


                                </div>
                            </div>
                        </div>
                    </div>";
            return html;
        }

        ///// <summary>
        ///// Generate standard input group with label & input tags
        ///// </summary>
        ///// <param name="name">Name & id for the input groups</param>
        ///// <param name="label">Text to display in label</param>
        ///// <param name="cannot_null">Set the input to be checked with validate_form</param>
        ///// <param name="type">Type of input (text, password, number, etc)</param>
        ///// <param name="value">Value to be passed in input group</param>
        ///// <returns></returns>
        //public static string get_input_group(string name, string label, bool cannot_null = false, string type = "text", string value = "")
        //{
        //    string cannot_null_value = cannot_null == false ? "" : "cannot-null";
        //    string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
        //    string input_group = @"
        //           <div class='form-group row'>
        //             <label class='col-md-3 col-form-label text-right' for='" + name + @"'>" + label + @" " + red_symbol + @"</label>
        //             <div class='col-md-9'>
        //               <input type='" + type + @"' id='" + name + @"' class='form-control " + cannot_null_value + @"' placeholder='" + label + @"' name='" + name + @"' value='" + value + @"'>
        //             </div>
        //           </div>";
        //    return input_group;
        //}



        //public static string get_rich_text_summurnote(string label, string name, bool cannot_null = false, string type = "text", string value = "")
        //{
        //    string cannot_null_value = cannot_null == false ? "" : "cannot-null";
        //    string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
        //    string rich_text_summurnote = @"
        //   <div class='form-group row'>
        //     <label class='col-md-3 col-form-label text-right' for='" + name + @"'>" + label + @" " + red_symbol + @"</label>
        //     <div class='col-md-9'>
        //       <textarea rows='3' class='form-control' id='" + name + @"' name='" + name + @"'></textarea>
        //     </div>
        //   </div>
        // <script>
        //	    $('#" + name + @"').summernote({
        //         height: 300,
        //         minheight: null,
        //         maxheight: null,
        //         focus: true
        //       }); //ckeditor.replace('" + name + @"'); 
        //    </script>";
        //    return rich_text_summurnote;
        //}

        /// <summary>
        /// Generate standard input
        /// </summary>
        /// <param name="name">Name & id for the input groups</param>
        /// <param name="type">Type of input (text, password, number, etc)</param>
        /// <param name="value">Value to be passed in input group</param>
        /// <returns></returns>
        public static string get_input(string name, string type = "text", string value = "")
        {
            string input = @"<input type='" + type + @"' id='" + name + @"' class='form-control' placeholder='" + value + @"' name='" + name + @"' value='" + value + @"'>";
            return input;
        }

        //     /// <summary>
        //     /// Generate select based on anonymous list sent from the view
        //     /// </summary>
        //     /// <param name="name"></param>
        //     /// <param name="label"></param>
        //     /// <param name="select_list">the anonymous list used to populate options</param>
        //     /// <param name="default_value">default selected index</param>
        //     /// <param name="disabled"></param>
        //     /// <returns></returns>
        //     public static string get_dropdown_group(string name, string label, dynamic select_list, string default_value = "-1", bool disabled = false)
        //     {
        //         bool cannot_null = true;
        //         string cannot_null_value = cannot_null == false ? "" : "cannot-null";
        //         string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";

        //         string disabled_string = disabled == false ? "" : "disabled";
        //         string select = "<select  name='" + name + "' id='" + name + "' class='form-control' " + disabled_string + ">";
        //         foreach (var select_list_data in select_list)
        //         {
        //             //logic to parsing anonymous list with propertyInfo looping
        //             Type t = select_list_data.GetType();
        //             PropertyInfo[] pi = t.GetProperties();

        //             //define what property to be assigned
        //             string select_value = pi[0].GetValue(select_list_data) + "";
        //             string select_display = pi[1].GetValue(select_list_data);

        //             //create select options
        //             select = select + "<option value='" + select_value + "' ";
        //             AppGlobal.console_log("select_value", select_value);
        //             AppGlobal.console_log("default_value", default_value);
        //             if (select_value + "" == default_value)
        //             {
        //                 select = select + "selected";
        //             }
        //             select = select + ">" + select_display + "</option>";
        //         }
        //         select = select + "</select>";

        //         string input_group = @"
        //   <div class='form-group row'>
        //     <label class='col-md-3 col-form-label text-right' for='" + name + @"'>" + label + @" " + red_symbol + @"</label>
        //     <div class='col-md-9'>
        //       " + select + @"
        //     </div>
        //   </div>";
        //         return input_group;
        //     }

        //     /// <summary>
        //     /// Generate switch group (checkbox modifier)
        //     /// Represented by hidden input to create consistency for backend post parameter
        //     /// </summary>
        //     /// <param name="name"></param>
        //     /// <param name="label"></param>
        //     /// <param name="_checked">the state of the checkbox</param>
        //     /// <param name="on_text">displayed text for on state</param>
        //     /// <param name="off_text">displayed text for off state</param>
        //     /// <returns></returns>
        //     public static string get_toggle_group(string name, string label, bool _checked = false, string on_text = "Ya", string off_text = "Tidak")
        //     {
        //         string checked_string = _checked == false ? "" : "checked";
        //         string input_group = @"
        //   <div class='form-group row'>
        //     <label class='col-md-3 col-form-label text-right' for='" + name + @"_area'>" + label + @"</label>
        //     <div class='col-md-9'>
        //       <input type='checkbox' name='" + name + @"_area' id='" + name + @"_area' class='form-control' " + checked_string +
        //             @" data-on-text='" + on_text + @"' data-off-text='" + off_text + @"' onchange='toggle_update(""" + name + @""")'>
        //       <input type='hidden' id='" + name + @"' class='form-control' name='" + name + @"' value='" + _checked + @"'>
        //     </div>
        //   </div>
        //   <script>
        //     $('#" + name + @"_area').bootstrapSwitch();
        //   </script>";
        //         return input_group;
        //     }

        //     //RFC : Add
        //     //1. calendar control
        //     //2. text area control
        //     //3. rich text control >> modify send_insert_data & send_edit_data to include custom data getter
        //     //4. image upload (with preview)
        //     //5. file upload (non preview)
    }
}