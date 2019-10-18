
function event_msg(data){
    show_msg(data);
    return ;
    while(data!=null && data!=""){
        if (data.startsWith("s:keep")){
            s_ui.status_label_show("status_label2","keep"+keep_count);
            keep_count++;
            var index=data.indexOf("\n");
            data=data.substring(index+1);
        }else if(data.startsWith("m:<s>:")){
            var index1=data.indexOf(":<s>:");
            var index2=data.indexOf(":</s>");
            if (index2>index1 && index1>0){
                var json=data.substring(index1+5,index2);
                json=json.replaceAll("\\r","\\\r");
                json=json.replaceAll("\\n","\\\n");
                show_msg(json);
                data=data.substring(index2+5);
                var index3=data.indexOf("\n");
                if (index3>=0) data=data.substring(index3+1);
            }else{
                break;
            }
        }else{
            break;
        }
    }
}
