[[[common.js]]]
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
    var a=sys.DataGrid_Read("grid1",index,0);
    
    sys.Msg(":"+a);
}

function clear_data(data){
    sys.DataGrid_Clear("grid1");
}

//界面
[[[ui.js]]]

data_init1("");

