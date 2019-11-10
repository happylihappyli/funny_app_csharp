//这个功能在窗口写：hello word
var content="";
function moue_hook(data){
    s_ui.text_set("txt","hook"); 
    s_ui.moue_hook("mouse_click",0,0,140,30);
    s_ui.top_most(1);
}

function moue_unhook(data){
    s_ui.text_set("txt","unhook"); 
    s_ui.moue_unhook();
}

function mouse_click(data){
    s_ui.text_set("txt",data); 
    
}

s_ui.button_init("b_hook","hook",10,10,100,30,"moue_hook","");
s_ui.button_init("b_unhook","unhook",10,50,100,30,"moue_hook","");

s_ui.textbox_init("txt","显示信息：",10,100,500,200);
s_ui.show_form(600,400);
s_ui.Form_Title("mouse hook");


