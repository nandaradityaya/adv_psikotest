Imports Adv

Public Class MenuAssignment

    Public Shared Function CheckRoleAssignment(ByVal Role As String, ByVal Path As String) As Boolean
        'Dokumen Angkutan
        Dim AllowedMenuAdminDA() As String = {"/DA/Home.aspx", _
                                             "/DA/Admin_viewDA.aspx", _
                                             "/DA/Admin_viewPost.aspx"}

        Dim AllowedMenuUserDA() As String = {"/DA/Home.aspx", _
                                        "/DA/User_view.aspx", _
                                        "/DA/User_export.aspx"}

        Dim AllowedMenuApprovalDA() As String = {"/DA/Home.aspx", _
                                      "/DA/Approval_viewApproval.aspx"}

        Dim AllowedMenuAuditDA() As String = {"/DA/Home.aspx", _
                                     "/DA/Audit_view.aspx"}

        Dim AllowedMenuSummaryDA() As String = {"/DA/Home.aspx", _
                                    "/DA/Summary_summaryDA.aspx"}

        Dim AllowedMenuCheckerDA() As String = {"/DA/Home.aspx", _
                                    "/DA/Checker_viewDA.aspx"}

        Dim AllowedMenuMarketingDA() As String = {"/DA/Home.aspx", _
                                      "/DA/Marketing_MasterCabang.aspx", _
                                      "/DA/Marketing_MasterCabangAdd.aspx", _
                                      "/DA/Marketing_MsClient.aspx", _
                                      "/DA/Marketing_MsImport.aspx"}

        Dim AllowedMenuReportRetail() As String = {"/DA/Home.aspx", _
                                   "/DA/RepRetail.aspx"}



        'Memorandum of Approval (MOA)
        Dim AllowedMenuAdminMOA() As String = {"/MOA/Home.aspx", _
                                            "/MOA/MsType.aspx", _
                                             "/MOA/Admin_eksportMOA.aspx", _
                                             "/MOA/Admin_viewMOA.aspx"}
        Dim AllowedMenuPurchasingMOA() As String = {"/MOA/Home.aspx", _
                                             "/MOA/Purchasing_purchs.aspx", _
                                             "/MOA/Purchasing_EksportExcel.aspx"}
        Dim AllowedMenuUserMOA() As String = {"/MOA/Home.aspx", _
                                             "/MOA/User_userMOA.aspx"}
        Dim AllowedMenuSuperAdminMOA() As String = {"/MOA/Home.aspx", _
                                                    "/MOA/SuperAdmin_ViewMOA.aspx"}
        Dim AllowedMenuRealisasiMOA() As String = {"/MOA/Home.aspx", _
                                                    "/MOA/Realisasi_ViewMoa.aspx"}


        'BON System
        Dim AllowedMenuAdminBON() As String = {"/BON/Home.aspx", _
                                            "/BON/Admin_MsACC.aspx", _
                                             "/BON/Admin_MsBus.aspx", _
                                             "/BON/Admin_MsCost.aspx", _
                                             "/BON/Admin_MsPlat.aspx", _
                                            "/BON/Admin_view.aspx", _
                                             "/BON/Admin_viewPost.aspx"}
        Dim AllowedMenuAuditBON() As String = {"/BON/Home.aspx", _
                                             "/BON/Audit_EksportEx.aspx", _
                                             "/BON/Audit_view.aspx"}
        Dim AllowedMenuUserBON() As String = {"/BON/Home.aspx", _
                                             "/BON/User_view.aspx"}
	'HRD Overtime
		Dim AllowedMenuCabangHRD() As String = {"/HRD/Home.aspx",
												"/HRD/HRD_Attendance.aspx",												
												"/HRD/HRD_EmployeeTrainingAttendance.aspx",
												"/HRD/HRD_HomestayReimbursement.aspx",
												"/HRD/HRD_AttendanceSummary.aspx",
												"/HRD/User_ViewOvertime.aspx"}

		Dim AllowedMenuApprovalHRD() As String = {"/HRD/Home.aspx",
												  "/HRD/HRD_Attendance.aspx",
												  "/HRD/HRD_EmployeeTrainingAttendance.aspx",
												  "/HRD/HRD_HomestayReimbursement.aspx",
												  "/HRD/HRD_AttendanceSummary.aspx",
												  "/HRD/Approval_ViewOvertime.aspx"}


		Dim AllowedMenuVerifikasiHRD() As String = {"/HRD/Home.aspx",
													"/HRD/HRD_Attendance.aspx",
													"/HRD/HRD_AttendanceSummary.aspx",
													"/HRD/HRD_EmployeeTrainingAttendance.aspx",
													"/HRD/HRD_HomestayReimbursement.aspx",
													"/HRD/Verifikator_ViewOvertime.aspx"}

		Dim AllowedMenuAdminHRD() As String = {"/HRD/Home.aspx",
											   "/HRD/HRD_Closing.aspx",
											   "/HRD/HRD_import.aspx",
											   "/HRD/HRD_export.aspx",
											   "/HRD/HRD_EmployeeTrainingAttendance.aspx",
											   "/HRD/HRD_HomestayReimbursement.aspx",
											   "/HRD/HRD_Attendance.aspx",
											   "/HRD/HRD_AttendanceSummary.aspx",
											   "/HRD/HRD_viewCabang.aspx"}

		Dim AllowedMenuAuditHRD() As String = {"/HRD/Home.aspx",
											   "/HRD/HRD_Attendance.aspx",
											   "/HRD/HRD_AttendanceSummary.aspx",
											   "/HRD/HRD_viewCabang.aspx"}

		Dim AllowedMenuFPDK_ADMIN() As String = {"/HRD/Home.aspx",
                                               "/HRD/HRD_MsBranch.aspx",
                                               "/HRD/HRD_MsBranchEdit.aspx",
                                               "/HRD/HRD_MsDivision.aspx",
                                               "/HRD/HRD_MsDivisionAddEdit.aspx",
                                               "/HRD/HRD_MsDepartment.aspx",
                                               "/HRD/HRD_MsDepartmentAddEdit.aspx",
                                               "/HRD/HRD_MsSection.aspx",
                                               "/HRD/HRD_MsSectionAddEdit.aspx",
                                               "/HRD/HRD_MsSubUnit.aspx",
                                               "/HRD/HRD_MsSubUnitAddEdit.aspx",
                                               "/HRD/HRD_MsPosition.aspx",
                                               "/HRD/HRD_MsPositionAddEdit.aspx",
                                               "/HRD/HRD_MsGrade.aspx",
                                               "/HRD/HRD_MsGradeAddEdit.aspx",
                                               "/HRD/HRD_TunjanganMutasiKaryawan.aspx",
                                               "/HRD/HRD_TunjanganMutasiKaryawanAddEdit.aspx",
                                               "/HRD/HRD_DivisionDepartmentMapping.aspx",
                                               "/HRD/HRD_DivisionDepartmentMappingEdit.aspx",
                                               "/HRD/HRD_DivisionDepartmentSectionMapping.aspx",
                                               "/HRD/HRD_DivisionDepartmentSectionMappingEdit.aspx",
                                               "/HRD/HRD_DivisionDepartmentSectionSubUnitMapping.aspx",
                                               "/HRD/HRD_DivisionDepartmentSectionSubUnitMappingEdit.aspx",
                                               "/HRD/HRD_DivisionPositionMapping.aspx",
                                               "/HRD/HRD_DivisionPositionMappingEdit.aspx",
                                               "/HRD/HRD_EmployeeExtendContractHRApproval.aspx",
                                               "/HRD/HRD_EmployeeExtendContractAddEdit.aspx",
                                               "/HRD/HRD_EmployeeExtendContractApproveReject.aspx",
                                               "/HRD/HRD_EmployeeExtendContractForceReject.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractHRApproval.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractApproveReject.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractForceReject.aspx",
                                               "/HRD/HRD_EmployeeTransferHRApproval.aspx",
                                               "/HRD/HRD_EmployeeTransferHRPayrollApproval.aspx",
                                               "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                               "/HRD/HRD_EmployeeTransferView.aspx",
                                               "/HRD/HRD_EmployeeTransferApproveRejectW.aspx",
                                               "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                               "/HRD/HRD_EmployeeTransferForceReject.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetViewDocuments.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetHRPayrollApproval.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetAddEdit.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetApproveRejectH.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetHRUpdate.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetForceReject.aspx",
                                               "/HRD/HRD_ImportExportMasterDataTraining.aspx",
                                               "/HRD/HRD_EmployeeTransferSelector.aspx",
                                               "/HRD/HRD_EmployeeTransactionReport.aspx"}


        Dim AllowedMenuFPDK_HRAPPROVAL() As String = {"/HRD/Home.aspx",
                                                "/HRD/HRD_EmployeeExtendContractHRApproval.aspx",
                                               "/HRD/HRD_EmployeeExtendContractAddEdit.aspx",
                                               "/HRD/HRD_EmployeeExtendContractApproveReject.aspx",
                                               "/HRD/HRD_EmployeeExtendContractForceReject.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractHRApproval.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractApproveReject.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractForceReject.aspx",
                                               "/HRD/HRD_EmployeeTransferHRApproval.aspx",
                                               "/HRD/HRD_EmployeeTransferHRPayrollApproval.aspx",
                                               "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                               "/HRD/HRD_EmployeeTransferView.aspx",
                                               "/HRD/HRD_EmployeeTransferApproveRejectW.aspx",
                                               "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                               "/HRD/HRD_EmployeeTransferForceReject.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetViewDocuments.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetHRPayrollApproval.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetHRApproval.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetAddEdit.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetApproveRejectH.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetHRUpdate.aspx",
                                               "/HRD/HRD_EmployeeClearanceSheetForceReject.aspx",
                                               "/HRD/HRD_ImportExportMasterDataTraining.aspx",
                                               "/HRD/HRD_EmployeeTransferSelector.aspx",
                                               "/HRD/HRD_EmployeeTransactionReport.aspx"}



        Dim AllowedMenuFPDK_BM() As String = {"/HRD/HRD_EmployeeExtendContractBMApproval.aspx",
                                                  "/HRD/HRD_EmployeeExtendContractAddEdit.aspx",
                                                  "/HRD/HRD_EmployeeExtendContractApproveReject.aspx",
                                                  "/HRD/HRD_EmployeeTrainingToContractBMApproval.aspx",
                                                  "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                                  "/HRD/HRD_EmployeeTrainingToContractApproveReject.aspx",
                                                  "/HRD/HRD_EmployeeTransferEntryBMRH.aspx",
                                                  "/HRD/HRD_EmployeeTransferOriginBMApproval.aspx",
                                                  "/HRD/HRD_EmployeeTransferDestinationBMApproval.aspx",
                                                  "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                                  "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                                  "/HRD/HRD_EmployeeTransferApproveRejectW.aspx",
                                                  "/HRD/HRD_EmployeeTransferView.aspx",
                                                  "/HRD/HRD_EmployeeClearanceSheetViewDocuments.aspx",
                                                  "/HRD/HRD_EmployeeClearanceSheetBMApproval.aspx",
                                                  "/HRD/HRD_EmployeeClearanceSheetAddEdit.aspx",
                                                  "/HRD/HRD_EmployeeClearanceSheetApproveReject.aspx",
                                                  "/HRD/HRD_EmployeeList.aspx",
                                                  "/HRD/HRD_EmployeeListDetail.aspx",
                                                  "/HRD/HRD_EmployeeTransferSelector.aspx",
                                                  "/HRD/HRD_EmployeeTransactionReport.aspx"}

        Dim AllowedMenuFPDK_RH() As String = {"/HRD/Home.aspx",
                                            "/HRD/HRD_EmployeeTransferEntryBMRH.aspx",
                                            "/HRD/HRD_EmployeeTransferOriginRHApproval.aspx",
                                            "/HRD/HRD_EmployeeTransferDestinationRHApproval.aspx",
                                            "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                            "/HRD/HRD_EmployeeTransferView.aspx",
                                            "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                            "/HRD/HRD_EmployeeTransferSelector.aspx",
                                            "/HRD/HRD_EmployeeTransferApproveRejectW.aspx",
                                            "/HRD/HRD_EmployeeList.aspx",
                                            "/HRD/HRD_EmployeeListDetail.aspx",
                                            "/HRD/HRD_EmployeeTransactionReport.aspx"}

        Dim AllowedMenuFPDK_OH() As String = {"/HRD/Home.aspx",
                                            "/HRD/HRD_EmployeeTransfer.aspx",
                                            "/HRD/HRD_EmployeeTransferOHApproval.aspx",
                                            "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                            "/HRD/HRD_EmployeeTransferView.aspx",
                                            "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                            "/HRD/HRD_EmployeeTransferSelector.aspx",
                                            "/HRD/HRD_EmployeeTransferApproveRejectW.aspx"}

        Dim AllowedMenuFPDK_TL() As String = {"/HRD/HRD_EmployeeExtendContract.aspx",
                                                    "/HRD/HRD_EmployeeExtendContractAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeExtendContractEmail.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContract.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContractUploadDocuments.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContractEmail.aspx",
                                                    "/HRD/HRD_EmployeeTransfer.aspx",
                                                    "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeTransferView.aspx",
                                                    "/HRD/HRD_EmployeeTransferEmail.aspx",
                                                    "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                                    "/HRD/HRD_EmployeeTransferDestinationApproval.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheetUploadDocuments.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheet.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheetAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheetEmail.aspx",
                                                    "/HRD/HRD_EmployeeTransferSelector.aspx",
                                                    "/HRD/HRD_EmployeeTransactionReport.aspx"}
													
		Dim AllowedMenuFPDK_MGR() As String = {"/HRD/HRD_EmployeeExtendContract.aspx",
                                                    "/HRD/HRD_EmployeeExtendContractAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeExtendContractEmail.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContract.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContractUploadDocuments.aspx",
                                                    "/HRD/HRD_EmployeeTrainingToContractEmail.aspx",
                                                    "/HRD/HRD_EmployeeTransfer.aspx",
                                                    "/HRD/HRD_EmployeeTransferAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeTransferView.aspx",
                                                    "/HRD/HRD_EmployeeTransferEmail.aspx",
                                                    "/HRD/HRD_EmployeeTransferApproveReject.aspx",
                                                    "/HRD/HRD_EmployeeTransferDestinationApproval.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheetUploadDocuments.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheet.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheetAddEdit.aspx",
                                                    "/HRD/HRD_EmployeeClearanceSheetEmail.aspx",
                                                    "/HRD/HRD_EmployeeTransferSelector.aspx",
                                                    "/HRD/HRD_EmployeeList.aspx",
                                                    "/HRD/HRD_EmployeeListDetail.aspx",
                                                    "/HRD/HRD_EmployeeTransactionReport.aspx"}													



        Dim AllowedMenuFPDK_IT() As String = {"/HRD/HRD_EmployeeClearanceSheetITApproval.aspx",
                                            "/HRD/HRD_EmployeeClearanceSheetAddEdit.aspx",
                                            "/HRD/HRD_EmployeeClearanceSheetApproveReject.aspx",
                                            "/HRD/HRD_EmployeeTransactionReport.aspx",
                                            "/HRD/HRD_EmployeeEmailRequestITUpdate.aspx",
                                            "/HRD/HRD_EmployeeEmailRequestITUpdatePopup.aspx"}


        Dim AllowedMenuFPDK_AUDIT() As String = {"/HRD/HRD_EmployeeTransactionReport.aspx"}


        Dim AllowedMenuFPDK_REC() As String = {"/HRD/HRD_EmployeeTrainingToContract.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractUploadDocuments.aspx",
                                               "/HRD/HRD_EmployeeTrainingToContractEmail.aspx"}



        Dim AllowedMenuFPDK_REC_APR() As String = {"/HRD/HRD_EmployeeTrainingToContractBMApproval.aspx",
                                                   "/HRD/HRD_EmployeeTrainingToContractAddEdit.aspx",
                                                   "/HRD/HRD_EmployeeTrainingToContractApproveReject.aspx"}



        'Risk Data Event
        Dim AllowedMenuUserRDE() As String = {"/RDE/Home.aspx", _
                                      "/RDE/User_user.aspx"}

        Dim AllowedMenuOPsRDE() As String = {"/RDE/Home.aspx", _
                                      "/RDE/OPS_viewRDE.aspx"}

        Dim AllowedMenuAdminNasRDE() As String = {"/RDE/Home.aspx", _
                                     "/RDE/Admin_viewRisk.aspx"}
        Dim AllowedMenuAproveRDE() As String = {"/RDE/Home.aspx", _
                                   "/RDE/Approval_viewApproval.aspx"}


        'User Request Form
        Dim AllowedMenuUserURF() As String = {"/URF/Home.aspx", _
                                      "/URF/User_view.aspx"}

        Dim AllowedMenuItdhURF() As String = {"/URF/Home.aspx", _
                                      "/URF/ITDH_view.aspx"}

        'Runsheet
        Dim AllowedMenuUserRunsheet() As String = _
            {"/RUN/RUNSHEET_UserView.aspx", _
             "/RUN/RUNSHEET_UserViewAddEdit.aspx", _
             "/RUN/RUNSHEET_InquiryView.aspx"}

        Dim AllowedMenuPusatRun() As String = _
             {"/RUN/RUNSHEET_AdminView.aspx", _
             "/RUN/RUNSHEET_InquiryView.aspx"}

        Dim AllowedMenuSPVRun() As String = _
            {"/RUN/RUNSHEET_SupervisorView.aspx", _
			 "/RUN/RUNSHEET_ViewOutstandingClosingCencon.aspx", _
             "/RUN/RUNSHEET_InquiryView.aspx"}

        Dim AllowedMenuBMRun() As String = _
           {"/RUN/RUNSHEET_SupervisorView.aspx", _
            "/RUN/RUNSHEET_InquiryView.aspx", _
			"/RUN/RUNSHEET_ViewOutstandingClosingCencon.aspx", _
            "/RUN/RUNSHEET_ViewCencon.aspx"}

        Dim AllowedMenuSPVMoncen() As String =
            {"/RUN/RUNSHEET_CancelAdminApprovedRunsheet.aspx", _
             "/RUN/RUNSHEET_CancelAdminApprovedRunsheetPopup.aspx", _
             "/RUN/RUNSHEET_InquiryView.aspx"}

        'SIMULATOR
        Dim AllowedMenuOPSimulator() As String = {"/SIMULATOR/Home.aspx", _
                                        "/SIMULATOR/ChangeSector.aspx", _
                                        "/SIMULATOR/RepSummaryProblem.aspx"}

        Dim AllowedMenuAproveSimulator() As String = {"/SIMULATOR/Home.aspx", _
                                       "/SIMULATOR/Simulator_SectorAssign.aspx"}

        Dim AllowedMenuReportSimulator() As String = {"/SIMULATOR/Home.aspx", _
                                       "/SIMULATOR/RepSummaryProblem.aspx"}

        'CPC
        Dim AllowedMenuOprCPC() As String = {"/CPC/Home.aspx", _
                                             "/CPC/Transaksi.aspx"}
        Dim AllowedMenuSpvCPC() As String = {"/CPC/Home.aspx", _
                                            "/CPC/VerifikasiDA.aspx", _
											"/CPC/Transaksi.aspx", _
                                             "/CPC/Report.aspx"}
        Dim AllowedMenuRptCPC() As String = {"/CPC/ReportRUDK.aspx"}


        'BBM
        Dim AllowedMenuInputerBBM() As String = {"/BBM/Home.aspx", _
                                         "/BBM/Inputer_ViewDecline.aspx", _
                                             "/BBM/Inputer_BBM.aspx"}
        Dim AllowedMenuSpvBBM() As String = {"/BBM/Home.aspx", _
                                              "/BBM/SPV_ViewDecline.aspx", _
                                             "/BBM/SPV_BBM.aspx"}
        Dim AllowedMenuAdminBBM() As String = {"/BBM/Home.aspx", _
                                            "/BBM/Admin_BBM.aspx", _
                                            "/BBM/Admin_PostingBBM.aspx", _
                                                "/BBM/Admin_MsJnsBB.aspx", _
                                            "/BBM/Admin_OpenApprove.aspx"}

        'Recording Questor and Cencon
        Dim AllowedMenuOperatorRQC() As String = _
            {"/RQC/RQC_KeyLoggerEntry.aspx", _
             "/RQC/RQC_KeyLoggerEntryAdd.aspx", _
             "/RQC/RQC_KeyLoggerClosing.aspx", _
             "/RQC/RQC_KeyLoggerInquiry.aspx"}

        Dim AllowedMenuAdminRQC() As String = _
            {"/RQC/RQC_MsKey.aspx", _
             "/RQC/RQC_MsKeyAddEdit.aspx", _
             "/RQC/RQC_MsProblem.aspx", _
             "/RQC/RQC_MsProblemAddEdit.aspx", _
             "/RQC/RQC_MsVault.aspx", _
             "/RQC/RQC_MsVaultAddEdit.aspx", _
             "/RQC/RQC_KeyLoggerEntry.aspx", _
             "/RQC/RQC_KeyLoggerEntryAdd.aspx", _
             "/RQC/RQC_KeyLoggerClosing.aspx", _
             "/RQC/RQC_KeyLoggerRevision.aspx", _
             "/RQC/RQC_KeyLoggerRevisionEdit.aspx", _
             "/RQC/RQC_KeyLoggerInquiry.aspx"}

        Dim AllowedMenuAuditRQC() As String = {"/RQC/RQC_KeyLoggerInquiry.aspx"}

        Dim AllowedMenuSmsRDB() As String = {"/Dashboard/Home.aspx", _
                                            "/Dashboard/RDB_SMS.aspx"}

        ' Role Dokumen Angkutan
        Select Case Role
            Case "DA_ADM"
                If Path.ContainsAny(AllowedMenuAdminDA) Then
                    Return True
                End If

            Case "DA_CBG"
                If Path.ContainsAny(AllowedMenuUserDA) Then
                    Return True
                End If

            Case "DA_APR"
                If Path.ContainsAny(AllowedMenuApprovalDA) Then
                    Return True
                End If
 
            Case "DA_CHK"
                If Path.ContainsAny(AllowedMenuCheckerDA) Then
                    Return True
                End If
 
            Case "DA_AUDIT"
                If Path.ContainsAny(AllowedMenuAuditDA) Then
                    Return True
                End If

            Case "DA_MRKT"
                If Path.ContainsAny(AllowedMenuMarketingDA) Then
                    Return True
                End If

            Case "DA_SUM"
                If Path.ContainsAny(AllowedMenuSummaryDA) Then
                    Return True
                End If
            Case "DA_CPUD"
                If Path.ContainsAny(AllowedMenuReportRetail) Then
                    Return True
                End If
        End Select


        ' Role Memorandum of Approval (MOA)
        Select Case Role
            Case "MOA_ADFIN"
                If Path.ContainsAny(AllowedMenuAdminMOA) Then
                    Return True
                End If

            Case "MOA_PURCH"
                If Path.ContainsAny(AllowedMenuPurchasingMOA) Then
                    Return True
                End If

            Case "MOA_ADMIN"
                If Path.ContainsAny(AllowedMenuSuperAdminMOA) Then
                    Return True
                End If

            Case "MOA_REALISASI"
                If Path.ContainsAny(AllowedMenuRealisasiMOA) Then
                    Return True
                End If

            Case "MOA_Cabang", "MOA_ADMMTG", "MOA_BPM", "MOA_CPCJKT", "MOA_FLMJKT", "MOA_FLT", "MOA_FLTGRD", _
                "MOA_SRG", "MOA_GA", "MOA_IT", "MOA_KUR", "MOA_MKT", "MOA_HRD", _
                "MOA_SSFM", "MOA_OPRREG", "MOA_RWAJKT", "MOA_SAA"
                If Path.ContainsAny(AllowedMenuUserMOA) Then
                    Return True
                End If
        End Select


        ' Role BON System
        Select Case Role
            Case "BON_AUDIT"
                If Path.ContainsAny(AllowedMenuAuditBON) Then
                    Return True
                End If

            Case "BON_CAB"
                If Path.ContainsAny(AllowedMenuUserBON) Then
                    Return True
                End If

            Case "BON_PUST"
                If Path.ContainsAny(AllowedMenuAdminBON) Then
                    Return True
                End If
        End Select

       'Role HRD Overtime
        Select Case Role
            Case "HRD_HRD"
                If Path.ContainsAny(AllowedMenuAdminHRD) Then
                    Return True
                End If
            Case "HRD_AUDIT"
                If Path.ContainsAny(AllowedMenuAuditHRD) Then
                    Return True
                End If
            Case "HRD_CABANG", "HRD_CABANGP"
                If Path.ContainsAny(AllowedMenuCabangHRD) Then
                    Return True
                End If
            Case "HRD_APR", "HRD_APRP"
                If Path.ContainsAny(AllowedMenuApprovalHRD) Then
                    Return True
                End If
            Case "HRD_VERO", "HRD_VEROP"
                If Path.ContainsAny(AllowedMenuVerifikasiHRD) Then
                    Return True
                End If
        End Select

               'Role HRD FPDK
        Select Case Role
            Case "FPDK_ADMIN"
                If Path.ContainsAny(AllowedMenuFPDK_ADMIN) Then
                    Return True
                End If
            Case "FPDK_HR_APPROVAL"
                If Path.ContainsAny(AllowedMenuFPDK_HRAPPROVAL) Then
                    Return True
                End If
            Case "FPDK_TL"
                If Path.ContainsAny(AllowedMenuFPDK_TL) Then
                    Return True
                End If
            Case "FPDK_MGR"
                If Path.ContainsAny(AllowedMenuFPDK_MGR) Then
                    Return True
                End If			
            Case "FPDK_BM", "FPDK_HEAD"
                If Path.ContainsAny(AllowedMenuFPDK_BM) Then
                    Return True
                End If
            Case "FPDK_RH"
                If Path.ContainsAny(AllowedMenuFPDK_RH) Then
                    Return True
                End If
            Case "FPDK_OH"
                If Path.ContainsAny(AllowedMenuFPDK_OH) Then
                    Return True
                End If
            Case "FPDK_IT"
                If Path.ContainsAny(AllowedMenuFPDK_IT) Then
                    Return True
                End If
            Case "FPDK_AUDIT"
                If Path.ContainsAny(AllowedMenuFPDK_AUDIT) Then
                    Return True
                End If
            Case "FPDK_REC"
                If Path.ContainsAny(AllowedMenuFPDK_REC) Then
                    Return True
                End If
            Case "FPDK_REC_APR"
                If Path.ContainsAny(AllowedMenuFPDK_REC_APR) Then
                    Return True
                End If
        End Select
		



        'Role Risk Data Event
        Select Case Role
            Case "RDE_OR", "RDE_AR"
                If Path.ContainsAny(AllowedMenuOPsRDE) Then
                    Return True
                End If

            Case "RDE_AN", "RDE_BPM"
                If Path.ContainsAny(AllowedMenuAdminNasRDE) Then
                    Return True
                End If

            Case "RDE_INP"
                If Path.ContainsAny(AllowedMenuUserRDE) Then
                    Return True
                End If

            Case "RDE_APR"
                If Path.ContainsAny(AllowedMenuAproveRDE) Then
                    Return True
                End If
        End Select


        'Role User Request Form (URF)
        Select Case Role
            Case "URF_USER"
                If Path.ContainsAny(AllowedMenuUserURF) Then
                    Return True
                End If

            Case "URF_ITDH", "URF_MAN"
                If Path.ContainsAny(AllowedMenuItdhURF) Then
                    Return True
                End If
        End Select

        'Role RUnsheet
        Select Case Role
            Case "RUN_CBG"
                If Path.ContainsAny(AllowedMenuUserRunsheet) Then
                    Return True
                End If

            Case "RUN_ADM"
                If Path.ContainsAny(AllowedMenuPusatRun) Then
                    Return True
                End If

            Case "RUN_SPV"
                If Path.ContainsAny(AllowedMenuSPVRun) Then
                    Return True
                End If

            Case "RUN_SPVMONCEN"
                If Path.ContainsAny(AllowedMenuSPVMoncen) Then
                    Return True
                End If

            Case "RUN_BM"
                If Path.ContainsAny(AllowedMenuBMRun) Then
                    Return True
                End If
        End Select

        'Role SIMULATOR
        Select Case Role
            Case "ASS_OP"
                If Path.ContainsAny(AllowedMenuOPSimulator) Then
                    Return True
                End If
            Case "ASS_APR"
                If Path.ContainsAny(AllowedMenuAproveSimulator) Then
                    Return True
                End If
            Case "ASS_RPT"
                If Path.ContainsAny(AllowedMenuReportSimulator) Then
                    Return True
                End If
        End Select

        'Role CPC
        Select Case Role
            Case "CPC_OPR"
                If Path.ContainsAny(AllowedMenuOprCPC) Then
                    Return True
                End If

            Case "CPC_SPV"
                If Path.ContainsAny(AllowedMenuSpvCPC) Then
                    Return True
                End If

            Case "CPC_RPT"
                If Path.ContainsAny(AllowedMenuRptCPC) Then
                    Return True
                End If
        End Select

        'Role BBM
        Select Case Role
            Case "BBM_INP"
                If Path.ContainsAny(AllowedMenuInputerBBM) Then
                    Return True
                End If

            Case "BBM_APR"
                If Path.ContainsAny(AllowedMenuSpvBBM) Then
                    Return True
                End If

            Case "BBM_ADM"
                If Path.ContainsAny(AllowedMenuAdminBBM) Then
                    Return True
                End If
        End Select

        'Role RQC
        Select Case Role
            Case "RQC_OP"
                If Path.ContainsAny(AllowedMenuOperatorRQC) Then
                    Return True
                End If

            Case "RQC_ADM"
                If Path.ContainsAny(AllowedMenuAdminRQC) Then
                    Return True
                End If

            Case "RQC_AUDIT"
                If Path.ContainsAny(AllowedMenuAuditRQC) Then
                    Return True
                End If
        End Select

        'Role Dashboard (RDB)
        Select Case Role
            Case "RDB_SMS"
                If Path.ContainsAny(AllowedMenuSmsRDB) Then
                    Return True
                End If
        End Select

        Return False
    End Function

    Public Shared Function CreateListOfAllowedMenus(ByVal Role As String) As String
        Dim sb As New Text.StringBuilder
        sb.Append("<ul class=""nav"">")
        sb.Append("<li><a href=""../home.aspx"">Home</a></li>")


        ' Halaman Dokumen Angkutan
        Select Case Role
            Case "DA_ADM"
                sb.Append("<li><a href=""Admin_viewDA.aspx"">View DA Cabang</a></li>")
                sb.Append("<li><a href=""Admin_viewPost.aspx"">Posting DA Cabang</a></li>")
            Case "DA_CBG"
                sb.Append("<li><a href=""User_view.aspx"">Entry Dokumen Angkutan</a></li>")
                sb.Append("<li><a href=""User_export.aspx"">Export DA</a></li>")
            Case "DA_APR"
                sb.Append("<li><a href=""Approval_viewApproval.aspx"">View DA Cabang</a></li>")
            Case "DA_AUDIT"
                sb.Append("<li><a href=""Audit_view.aspx"">View DA Cabang</a></li>")
            Case "DA_SUM"
                sb.Append("<li><a href=""Summary_summaryDA.aspx"">View DA Cabang</a></li>")
            Case "DA_MRKT"
                sb.Append("<li><a href=""Marketing_MasterCabang.aspx"">Master Cabang</a></li>")
                sb.Append("<li><a href=""Marketing_MsClient.aspx"">Master Klien</a></li>")
                sb.Append("<li><a href=""Marketing_MsImport.aspx"">Import Master Klien</a></li>")
            Case "DA_CPUD"
                sb.Append("<li><a href=""RepRetail.aspx"">Report</a></li>")
        End Select


        ' Halaman Memorandum of Approval (MOA)
        Select Case Role
            Case "MOA_ADFIN"
                sb.Append("<li><a href=""MsType.aspx"">Master Type</a></li>")
                sb.Append("<li><a href=""Admin_viewMOA.aspx"">View MOA</a></li>")
                sb.Append("<li><a href=""Admin_eksportMOA.aspx"">Eksport MOA</a></li>")
            Case "MOA_ADMIN"
                sb.Append("<li><a href=""SuperAdmin_ViewMOA.aspx"">View MOA</a></li>")
            Case "MOA_REALISASI"
                sb.Append("<li><a href=""Realisasi_ViewMoa.aspx"">View MOA</a></li>")
            Case "MOA_PURCH"
                sb.Append("<li><a href=""Purchasing_purchs.aspx"">View MOA</a></li>")
                sb.Append("<li><a href=""Purchasing_EksportExcel.aspx"">Eksport MOA</a></li>")
            Case "MOA_Cabang", "MOA_ADMMTG", "MOA_BPM", "MOA_CPCJKT", "MOA_FLMJKT", "MOA_FLT", "MOA_FLTGRD", _
                "MOA_SRG", "MOA_GA", "MOA_IT", "MOA_KUR", "MOA_MKT", "MOA_HRD", _
                "MOA_SSFM", "MOA_OPRREG", "MOA_RWAJKT", "MOA_SAA"
                sb.Append("<li><a href=""User_userMOA.aspx"">View MOA</a></li>")
        End Select


        ' Halaman BON Sys
        Select Case Role
            Case "BON_AUDIT"
                sb.Append("<li><a href=""Audit_view.aspx"">View BON</a></li>")
                sb.Append("<li><a href=""Audit_EksportEx.aspx"">Eksport Excel</a></li>")
            Case "BON_PUST"
                sb.Append("<li><a href=""Admin_view.aspx"">View BON Cabang</a></li>")
                sb.Append("<li><a href=""Admin_MsACC.aspx"">Master Acc Code</a></li>")
                sb.Append("<li><a href=""Admin_MsCost.aspx"">Master Cost Center</a></li>")
                sb.Append("<li><a href=""Admin_MsBus.aspx"">Master Bus Unit</a></li>")
                sb.Append("<li><a href=""Admin_MsPlat.aspx"">Master Plat No</a></li>")
                sb.Append("<li><a href=""Admin_viewPost.aspx"">Posting Cabang</a></li>")
            Case "BON_CAB"
                sb.Append("<li><a href=""User_view.aspx"">View BON</a></li>")
        End Select


        'Halaman HRD Overtime
        Select Case Role
            Case "HRD_CABANG", "HRD_CABANGP"
                sb.Append("<li><a href=""User_ViewOvertime.aspx"">View Overtime</a></li>")
            Case "HRD_VERO", "HRD_VEROP"
                sb.Append("<li><a href=""Verifikator_ViewOvertime.aspx"">View Overtime</a></li>")
            Case "HRD_APR", "HRD_APRP"
                sb.Append("<li><a href=""Approval_ViewOvertime.aspx"">View Overtime</a></li>")
            Case "HRD_HRD"
                sb.Append("<li><a href=""HRD_viewCabang.aspx"">View Overtime Cabang</a></li>")
                sb.Append("<li><a href=""HRD_Closing.aspx"">Closing Overtime Cabang</a></li>")
                sb.Append("<li><a href=""HRD_import.aspx"">Import</a></li>")
                sb.Append("<li><a href=""HRD_export.aspx"">Eksport</a></li>")
            Case "HRD_AUDIT"
                sb.Append("<li><a href=""HRD_viewCabang.aspx"">View Overtime Cabang</a></li>")
        End Select


        'Halaman Risk Data Event
        Select Case Role
            Case "RDE_OR", "RDE_AR"
                sb.Append("<li><a href=""OPS_viewRDE.aspx"">View Risk Event</a></li>")
            Case "RDE_AN", "RDE_BPM"
                sb.Append("<li><a href=""Admin_viewRisk.aspx"">View Risk Event Cabang</a></li>")
            Case "RDE_INP"
                sb.Append("<li><a href=""User_user.aspx"">Entry Risk Event</a></li>")
            Case "RDE_APR"
                sb.Append("<li><a href=""Approval_viewApproval.aspx"">View Risk Event</a></li>")
        End Select


        'Halaman User Request Form
        Select Case Role
            Case "URF_USER"
                sb.Append("<li><a href=""User_view.aspx"">Entry URF</a></li>")
            Case "URF_ITDH", "URF_MAN"
                sb.Append("<li><a href=""ITDH_view.aspx"">View URF</a></li>")
        End Select


        'Halaman Runsheet
        Select Case Role
            Case "RUN_CBG"
                sb.Append("<li><a href=""Runsheet.aspx"">View Runsheet</a></li>")
                sb.Append("<li><a href=""User_Report.aspx"">Report</a></li>")
            Case "RUN_ADM"
                sb.Append("<li><a href=""Admin_ViewRun.aspx"">View Runsheet Cabang</a></li>")
                sb.Append("<li><a href=""Admin_ViewCancel.aspx"">View Runsheet Cancel</a></li>")
            Case "RUN_SPV"
                sb.Append("<li><a href=""SPV_RunSheetApprove.aspx"">Approve Runsheet</a></li>")
                sb.Append("<li><a href=""SPV_Runsheet.aspx"">Runsheet Cancel</a></li>")
            Case "RUN_SPVMONCEN"
                sb.Append("<li><a href=""SPV_Moncen.aspx"">View Runsheet Cabang</a></li>")
                sb.Append("<li><a href=""SPVMONCEN_Cancel.aspx"">View Runsheet Cancel Cabang</a></li>")
        End Select

        'Halaman Simulator
        Select Case Role
            Case "ASS_OP"
                sb.Append("<li><a href=""ChangeSector.aspx"">Master Data Sektor</a></li>")
                sb.Append("<li><a href=""RepSummaryProblem.aspx"">Laporan</a></li>")
            Case "ASS_APR"
                sb.Append("<li><a href=""Simulator_SectorAssign.aspx"">Sektor Assigment</a></li>")
            Case "ASS_RPT"
                sb.Append("<li><a href=""RepSummaryProblem.aspx"">Laporan</a></li>")
        End Select

        'Halaman Runsheet
        Select Case Role
            Case "CPC_OPR"
                sb.Append("<li><a href=""Transaksi.aspx"">Transaksi</a></li>")
            Case "CPC_SPV"
                sb.Append("<li><a href=""Transaksi.aspx"">Transaksi</a></li>")
                sb.Append("<li><a href=""Report.aspx"">Report</a></li>")
        End Select

        'Halaman BBM
        Select Case Role
            Case "BBM_INP"
                sb.Append("<li><a href=""Inputer_BBM.aspx"">View BBM Usage</a></li>")
                sb.Append("<li><a href=""Inputer_ViewDecline.aspx"">View Decline</a></li>")
            Case "BBM_APR"
                sb.Append("<li><a href=""SPV_BBM.aspx"">View BBM Usage</a></li>")
                sb.Append("<li><a href=""SPV_ViewDecline.aspx"">View Decline</a></li>")
            Case "BBM_ADM"
                sb.Append("<li><a href=""Admin_BBM.aspx"">View BBM Usage Cabang</a></li>")
                sb.Append("<li><a href=""Admin_PostingBBM.aspx"">Posting BBM Usage Cabang</a></li>")
                sb.Append("<li><a href=""Admin_OpenApprove.aspx"">Open Approve BBM Usage Cabang</a></li>")
                sb.Append("<li><a href=""Admin_MsJnsBB.aspx"">Master Jenis Bahan Bakar</a></li>")
        End Select

        'Halaman RQC
        Select Case Role

            Case "RQC_OP"
                sb.Append("<li><a href=""OP_CenconPage.aspx"">Pencatatan Cencon</a></li>")
                sb.Append("<li><a href=""OP_ViewCLoseCencon.aspx"">View Cencon Close</a></li>")
                sb.Append("<li><a href=""OP_QuestorPage.aspx"">Pencatatan Questor</a></li>")
                sb.Append("<li><a href=""OP_ViewCLoseQuestor.aspx"">View Questor Close</a></li>")
            Case "RQC_ADM"
                sb.Append("<li><a href=""Admin_CenconPage.aspx""> Cencon</a></li>")
                sb.Append("<li><a href=""Admin_QuestorPage.aspx""> Questor</a></li>")
                sb.Append("<li><a href=""Admin_MsKota.aspx"">Master Kota</a></li>")
                sb.Append("<li><a href=""Admin_MsLocation.aspx"">Master Lokasi</a></li>")
                sb.Append("<li><a href=""Admin_MsATM.aspx"">Master ATM</a></li>")
                sb.Append("<li><a href=""Admin_MsRF.aspx"">Master R/F</a></li>")
                sb.Append("<li><a href=""Admin_MsProblem.aspx"">Master Problem</a></li>")
                sb.Append("<li><a href=""Admin_MsCustody.aspx"">Master Custody</a></li>")
            Case "RQC_AUDIT"
                sb.Append("<li><a href=""Audit_ViewCencon.aspx""> Cencon</a></li>")
                sb.Append("<li><a href=""Audit_ViewQuestor.aspx""> Questor</a></li>")

        End Select

        'Halaman Dashboard
        Select Case Role
            Case "RDB_SMS"
                sb.Append("<li><a href=""RDB_SMS.aspx"">View Traffic SMS</a></li>")
        End Select

        sb.Append("</ul>")

        Return sb.ToString
    End Function

End Class