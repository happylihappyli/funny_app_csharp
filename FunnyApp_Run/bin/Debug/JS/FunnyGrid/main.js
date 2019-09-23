[[[..\\data\\common_string.js]]]
[[[tools.js]]]
[[[data.js]]]
[[[correlation.js]]]

//统计检验
[[[test.js]]]

//绘图
[[[draw.js]]]

function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.datagrid_Read("grid1",index,0);
    
    s_ui.msg(":"+a);
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

//界面
[[[ui.js]]]

data_init1("");

