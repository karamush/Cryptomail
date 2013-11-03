<?php
Include_once ('mybase.class.php');
$mail=trim($_POST['mail']);
$MyBaseReg = new MyBase();
echo $MyBaseReg->getKey($mail);
unset ($MyBaseReg);
?>