<?php
Include_once ('mybase.class.php');
$mail=trim($_POST['mail']);
$MyBaseReg = new MyBase();
$SID =trim($_POST['pubkey']);
$SID = str_replace(' ','+',$SID);
$MyBaseReg->UpdatePubKey($SID,$mail);
unset ($MyBaseReg);
echo 'OK';
?>