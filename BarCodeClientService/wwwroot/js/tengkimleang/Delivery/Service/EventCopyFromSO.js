let EventCopyFromPOService = {
    iEventCopyFromPO: new IEventCopyFromPO(),
    ChoosePO_Click(temporyListPO, TbCopyFromPO) {
        let response = EventCopyFromPOService.iEventCopyFromPO.ChoosePO_Click(temporyListPO, TbCopyFromPO);
        //console.log("EventCopyFromSO ->" + temporyListPO + "," + TbCopyFromPO)
        return response;
    },
    ChoosePONofity_Click(temporyListPO, TbCopyFromPO) {
        let response = EventCopyFromPOService.iEventCopyFromPO.ChoosePONofity_Click(temporyListPO, TbCopyFromPO);
        console.log("EventCopyFromSO_Nofity ->" + temporyListPO + "," + TbCopyFromPO)
        return response;
    }
}