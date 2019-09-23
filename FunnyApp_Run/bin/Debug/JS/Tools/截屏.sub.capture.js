
data=s_sys.value_read("sleep");
var isec=parseInt(data);
if (isec>0){
    s_sys.sleep(isec);
}