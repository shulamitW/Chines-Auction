
import React, { useState, useEffect, useRef } from 'react';
import { FilterMatchMode, FilterOperator } from 'primereact/api';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast'
import axios from 'axios';
import 'primeicons/primeicons.css';
import Home from './menuBarManager';


export default function PurchasesManagement() {
    const [expandedRows, setExpandedRows] = useState(null);
    const [purchases, setPurchases] = useState(null);
    const [presents, setPresents] = useState(null);
    const toast = useRef(null);
    const [loading, setLoading] = useState(true);
    const [presentPurchasesList, setPresentPurchasesList] = useState([]);
    const data = useRef([])

    const [filters, setFilters] = useState({
        name: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
        description: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
        price: { value: null, matchMode: FilterMatchMode.GREATER_THAN_OR_EQUAL_TO }

    });
 
    const GetPresentWithPurchases = async (tmp) => {
        let presentPurchaseTmp = [];
        
        for (let i = 0; i < tmp.length; i++) {
          const updatedObject = await aa(tmp[i]);
          presentPurchaseTmp.push(updatedObject);
        }
        setPresentPurchasesList(presentPurchaseTmp);
      };
      
      const aa = async (tmp) => {
        const purchasesList = await getPurchaseByPresent(tmp.id);
        const updatedObject = { ...tmp, purchase: purchasesList };
        
        return updatedObject;
    };

    useEffect(() => {

        getAllPresents();
        //GetPresentWithPurchases();
        setLoading(false);
    }, []);

    const getPurchaseByPresent = async (presentId) => {

        try {
            return await axios.get(`/Purchase/GetPurchaseByPresent?presentId=${presentId}`).then(res => {
                console.log(res.data);
                return res.data
            })
            //    console.log(tmp)
            //    setPurchases(tmp)     
        }
        catch (err) {
            console.log(err);
        }
    }


    const getAllPresents = async () => {

        try {
            let tmp = await axios.get(`/Present/GetPresents`).then(res => {
                console.log(res.data);
                return res.data
            })
            console.log(tmp)
            setPresents(tmp)
            await GetPresentWithPurchases(tmp)
        }
        catch (err) {
            console.log(err);
        }
    }

    const onRowExpand = (event) => {
        toast.current.show({ severity: 'info', summary: 'Product Expanded', detail: event.data.name, life: 3000 });
    };

    const onRowCollapse = (event) => {
        toast.current.show({ severity: 'success', summary: 'Product Collapsed', detail: event.data.name, life: 3000 });
    };

    const expandAll = () => {
        let _expandedRows = {};

        presentPurchasesList.forEach((p) => (_expandedRows[`${p.id}`] = true));

        setExpandedRows(_expandedRows);
    };

    const collapseAll = () => {
        setExpandedRows(null);
    };

    const formatCurrency = (value) => {
        return value.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
    };

    const priceBodyTemplate = (rowData) => {
        return formatCurrency(rowData.price);
    };

    const allowExpansion = (rowData) => {

       // return setPresentPurchasesList.purchases.length > 0;
        return true
        //return rowData.presents.length > 0;
    };

    const rowExpansionTemplate =  (data) => {
        console.log('-------------------------daaaaaaaaaaaaaaaaaataaaaaaaa', data);
        // console.log('purchasessssssss',purchases);
        return (
            data.purchase.length==0?<>No Purchases Found</>:
            <div className="p-3">
                <h5>Purchases Of {data.name}</h5>
                <DataTable value={data.purchase}>
                    <Column field="userId" header="User id" sortable></Column>
                    <Column field="dateOfPurchase" header="Date of purchase" sortable></Column>
                    {/* <Column field="typeOfPayment" header="type of payment" sortable ></Column>
                        <Column field="category.description" header="Category" sortable></Column>
                        <Column field="ImagePath" header="ImagePath" sortable></Column> 
                    */}
                    {/* <Column field="amount" header="Amount" body={amountBodyTemplate} sortable></Column>
                        <Column field="status" header="Status" body={statusOrderBodyTemplate} sortable></Column> */}
                    {/* <Column headerStyle={{ width: '4rem' }} body={searchBodyTemplate}></Column> */}
                </DataTable>
            </div>
        );
    }

    const renderHeader = () => {
        return (
            <div className="flex justify-content-end">
                {/* <IconField iconPosition="left"> */}
                {/* <InputIcon className="pi pi-search" />
                        <InputText value={globalFilterValue} onChange={onGlobalFilterChange} placeholder="Keyword Search" /> */}
                <div className="flex flex-wrap justify-content-end gap-2">
                    <Button icon="pi pi-plus" label="Expand All" onClick={expandAll} text />
                    <Button icon="pi pi-minus" label="Collapse All" onClick={collapseAll} text />

                </div>
                {/* </IconField> */}
            </div>
        );
    };
    const header = renderHeader();

    return (
        <>
        <Home activeIndex = {1} />
            <div className="card">
                <Toast ref={toast} />
                <DataTable value={presentPurchasesList} expandedRows={expandedRows} onRowToggle={(e) => setExpandedRows(e.data)}
                    onRowExpand={onRowExpand} onRowCollapse={onRowCollapse} rowExpansionTemplate={rowExpansionTemplate}
                    dataKey="id" header={header} tableStyle={{ minWidth: '60rem' }}
                    paginator rows={10} dataKey="id" filters={filters} filterDisplay="row" loading={loading}
                    globalFilterFields={['name', 'representative.name', 'status']} header={header} emptyMessage="No purchases found." >>
                    <Column field="name" header="Name" filter filterPlaceholder="Search by name" style={{ maxWidth: '11rem' }} />
                    <Column field="description" header="description"style={{ minWidth: '12rem' }} />
                    <Column field="category.description" header="category" style={{ minWidth: '12rem' }} />
                    <Column field="price" header="price" sortable style={{ minWidth: '12rem' }} />
                    <Column expander={allowExpansion} style={{ width: '5rem' }} />
                </DataTable>
            </div>
        </>
    )
}