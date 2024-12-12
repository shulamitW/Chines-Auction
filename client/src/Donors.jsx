
import React, { useState, useEffect,useRef } from 'react';
import { classNames } from 'primereact/utils';
import { FilterMatchMode, FilterOperator } from 'primereact/api';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { InputText } from 'primereact/inputtext';
import { IconField } from 'primereact/iconfield';
import { InputIcon } from 'primereact/inputicon';
import { MultiSelect } from 'primereact/multiselect';
import { Button } from 'primereact/button';
import { Tag } from 'primereact/tag';
import { Toast } from 'primereact/toast'
import axios from 'axios';
import { ButtonGroup } from 'primereact/buttongroup';
import 'primeicons/primeicons.css';
import { SpeedDial } from 'primereact/speeddial';
import { Dialog } from 'primereact/dialog';
import { RadioButton } from 'primereact/radiobutton';
import { InputTextarea } from 'primereact/inputtextarea';
import { InputNumber } from 'primereact/inputnumber';
import {InputMask} from 'primereact/inputmask'
import Home from './menuBarManager';
axios.defaults.baseURL="http://localhost:5018"

export default function Donors() {

    let emptyProduct = {
        id:null,
        firstName: '',
        lastName: '',
        imagePath: '',
        email: '',
        phone: '',
        address: '',
    };

    const [deleteProductsDialog, setDeleteProductsDialog] = useState(false);
    const [selectedProducts, setSelectedProducts] = useState(null);
    const [submitted, setSubmitted] = useState(false);
    const [globalFilter, setGlobalFilter] = useState(null);
    const dt = useRef(null);
    const [products, setProducts] = useState([]);
    const [expandedRows, setExpandedRows] = useState(null);
    const toast = useRef(null);    
    const [loading, setLoading] = useState(true);
    const [productDialog, setProductDialog] = useState(false);
    const [deleteProductDialog, setDeleteProductDialog] = useState(false);
    const [donor, setDonor] = useState(emptyProduct);
    const [globalFilterValue, setGlobalFilterValue] = useState('');
    const [sender, setSender] = useState('new');
    const [donors, setDonors] = useState(null);
    const [filters, setFilters] = useState({ 
        fullName: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
        email: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
        phone: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
        address: { value: null, matchMode: FilterMatchMode.STARTS_WITH }

    });

    const onRowExpand = (event) => {
        toast.current.show({ severity: 'info', summary: 'Product Expanded', detail: event.data.name, life: 3000 });
    };

    const onRowCollapse = (event) => {
        toast.current.show({ severity: 'success', summary: 'Product Collapsed', detail: event.data.name, life: 3000 });
    };

    const expandAll = () => {
        let _expandedRows = {};

        donors.forEach((p) => (_expandedRows[`${p.id}`] = true));

        setExpandedRows(_expandedRows);
    };

    const collapseAll = () => {
        setExpandedRows(null);
    };

    const formatCurrency = (value) => {
        return value.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
    };

    const donorIcon = (rowData) => {
        console.log('donorIcon',rowData);
        return (
            <React.Fragment>
                {/* <Button icon="pi pi-pencil" rounded outlined className="mr-2" onClick={() => editDonor(rowData.id,rowData)} /> */}
                <Button icon="pi pi-pencil" rounded outlined className="mr-2" onClick={() => editProduct(rowData.id,rowData)} />
                <Button icon="pi pi-trash" rounded outlined severity="danger" onClick={() => deleteDonor(rowData.id)} />
            </React.Fragment>
        );
    };

    const deleteDonor = async (donorId) => {

        try{
            setDeleteProductDialog(false);

            console.log("--- deleted donor --- " , donorId)
                
          await axios.delete(`/Donor/RemoveDonor/${donorId}`)
          .then((res)=>{
            //debugger
            console.log("--- deleted donor --- " , donorId)
            getAllDonors()        
            })  }
            catch(err){
                alert(err)
              }
    }  
      
    const saveProduct = () => {
        setSubmitted(true);
        if (donor.firstName.trim() && donor.lastName.trim() && donor.phone.trim() && donor.address.trim() && donor.email.trim()) 
        {  
            if(sender==='edit')
            {
                const tmpDonor={
                    firstName: donor.firstName,
                    lastName: donor.lastName,
                    imagePath: donor.imagePath,
                    email: donor.email,
                    phone: donor.phone,
                    address: donor.address,
                }
                editDonor(donor.id,tmpDonor);
                toast.current.show({ severity: 'success', summary: 'Successful', detail: 'Product Updated', life: 3000 });
            }
            else{ //if sender=='new'
                addDonor(donor)
            }
        }
        else//if there is null
            toast.current.show({severity: 'error', summary: 'Error Message', detail: 'Validation failed'});  
        // setProducts(_products);
        setProductDialog(false);
        setDonor(emptyProduct);

    }

    const editProduct = (productId,donor) => {
        {console.log('donor in edit-------------',donor);}
        setDonor({ ...donor });
        setProductDialog(true);
        setSender("edit")
    };
    // const onInputNumberChange = (e, name) => {
    //     const val
    //     }
    //     catch(err){
    //       alert(err)
    //     }
    // }
   
    const  addDonor  = async(donor) => {

        try{
            console.log("--- add donor --- " , donor.firstName)
                
          await axios.post(`/Donor/AddDonor`,donor)
          .then(res=>console.log(res.data.firstName))
          getAllDonors()     
        }
        catch(err){
          alert(err)
        }
    }
   
     
    //************************************************************ */
    // useEffect(() => {
    //     getAllDonors();
    //     setLoading(false);
    // }, [deleteDonor,editDonor]); 
    //************************************************************ */

    // const searchBodyTemplate = () => {
    //     return <Button icon="pi pi-search" />;
    // };

    // const SpeedDialIcon = (donor) => {
    //     return <div className="card">
    //     {console.log('donor',donor)}
    //                  <div style={{ position: 'relative', height: '200px' ,minWidth: '12rem'}}>
    //                    <Toast ref={toast} />
    //                      <SpeedDial size="big" model={items} direction="up" transitionDelay={80} showIcon="pi pi-bars" hideIcon="pi pi-times" buttonClassName="p-button-outlined" />
    //                 </div>
    //             </div>;
    // };
    // const imageBodyTemplate = (rowData) => {
    //     return <img src={rowData.image} alt={rowData.image} width="64px" className="shadow-4" />;
    // };


    const priceBodyTemplate = (rowData) => {
        return formatCurrency(rowData.price);
    };

    const allowExpansion = (rowData) => {
        
        return rowData.presents.length > 0;
    };

    const rowExpansionTemplate = (data) => {
        return (
            <div className="p-3">
                <h5>Presents Of {data.fullName}</h5>
                <DataTable value={data.presents}>
                    {/* <Column field="id" header="Id" sortable></Column> */}
                    <Column field="name" header="Name" sortable></Column>
                    <Column field="description" header="Description" sortable></Column>

                    <Column field="price" header="Price" sortable body={priceBodyTemplate}></Column>
                    <Column field="category.description" header="Category" sortable></Column>
                    <Column field="ImagePath" header="ImagePath" sortable></Column>

                    {/* <Column field="amount" header="Amount" body={amountBodyTemplate} sortable></Column>
                    <Column field="status" header="Status" body={statusOrderBodyTemplate} sortable></Column> */}
                    {/* <Column headerStyle={{ width: '4rem' }} body={searchBodyTemplate}></Column> */}
                </DataTable>
            </div>
        );
    };
    
    
    const openNew = () => {
        setDonor(emptyProduct);
        setSubmitted(false);
        setProductDialog(true);
        setSender("new")

    };

    const hideDialog = () => {
        setSubmitted(false);
        setProductDialog(false);
    };
    
    const getAllDonors= async()=>
    {
        try{
           let tmp= await axios.get(`/Donor/GetDonors`).then(res=>
            {
                console.log(res.data);
                return res.data
            }) 
           console.log(tmp)
           setDonors(tmp)     
        }   
        catch(err){
            alert(err)
        }
    }
       
    useEffect(() => {
        getAllDonors();
        setLoading(false);
    }, []); // eslint-disable-line react-hooks/exhaustive-deps
    

    const onCategoryChange = (e) => {
        let _product = { ...donor };

        _product['category'] = e.value;
        setDonor(_product);
    };
    const onGlobalFilterChange = (e) => {
        const value = e.target.value;
        let _filters = { ...filters };

        _filters['global'].value = value;

        setFilters(_filters);
        setGlobalFilterValue(value);
    };


    const editDonor = async (donorId,donor) => {
        try{

          await axios.put(`/Donor/UpdateDonor/${donorId}`,donor)
          .then((res)=>{
            console.log("--- edit donor --- " , donorId)
        })    
        await getAllDonors() 
        }
        catch(err){
          alert(err)
        }
    } 

//  = e.value || 0;
//         let _product = { ...donor };

//         _product[`${name}`] = val;

//         setDonor(_product);
//     };
    const onInputChange = (e, name) => {
        const val = (e.target && e.target.value) || '';
        let _product = { ...donor };

        _product[`${name}`] = val;

        setDonor(_product);
    };

    const productDialogFooter = (
        <React.Fragment>
            <Button label="Cancel" icon="pi pi-times" outlined onClick={hideDialog} />
            <Button label="Save" icon="pi pi-check" onClick={saveProduct} />
        </React.Fragment>
    );
    const renderHeader = () => {
        return (
            <div className="flex justify-content-end">
                {/* <IconField iconPosition="left"> */}
                    {/* <InputIcon className="pi pi-search" />
                    <InputText value={globalFilterValue} onChange={onGlobalFilterChange} placeholder="Keyword Search" /> */}
                    <div className="flex flex-wrap justify-content-end gap-2">
                    <Button icon="pi pi-plus" label="Expand All" onClick={expandAll} text />
                    <Button icon="pi pi-minus" label="Collapse All" onClick={collapseAll} text />
                    <Button  icon="pi pi-plus" label="New" onClick={openNew} />
                    </div>
                {/* </IconField> */}
            </div>
        );
    };
    const header = renderHeader();

    // const deleteProductDialogFooter = (
    //     <React.Fragment>
    //         <Button label="No" icon="pi pi-times" outlined onClick={hideDeleteProductDialog} />
    //         <Button label="Yes" icon="pi pi-check" severity="danger" onClick={confirmDeleteProduct} />
    //     </React.Fragment>
    // );

    
    // const confirmDeleteProduct = () => {
    //     //setDonor(donor);  ???
    //     deleteDonor()
    //     setDeleteProductDialog(true);
    // };
    // const hideDeleteProductDialog = () => {
    //     setDeleteProductDialog(false);
    // };

    
    return (
        <>
        <Home activeIndex = {2} />
        <div className="card">
            <Toast ref={toast} />
            <DataTable  value={donors} expandedRows={expandedRows} onRowToggle={(e) => setExpandedRows(e.data)}
                    onRowExpand={onRowExpand} onRowCollapse={onRowCollapse} rowExpansionTemplate={rowExpansionTemplate}
                    dataKey="id" header={header} tableStyle={{ minWidth: '60rem' }}
                    paginator rows={10} dataKey="id" filters={filters} filterDisplay="row" loading={loading}
                    globalFilterFields={['name', 'representative.name', 'status']} header={header} emptyMessage="No donors found.">>

                <Column field="fullName" header="Name" filter filterPlaceholder="Search by name" style={{ minWidth: '12rem' }} />
                <Column field="address" header="Address" filter filterPlaceholder="Search by address" style={{ minWidth: '12rem' }} />
                <Column field="phone" header="Phone" filter filterPlaceholder="Search by phone" style={{ minWidth: '12rem' }} />
                <Column field="email" header="Email" filter filterPlaceholder="Search by email" style={{ minWidth: '12rem' }} />
                {/* <Button label="Save" icon="pi pi-check" /> */}
                <Column body={donorIcon} exportable={false} style={{ minWidth: '12rem' }}></Column>
                <Column expander={allowExpansion} style={{ width: '5rem' }} />
            
            </DataTable>
        </div>

        {/* <Dialog visible={deleteProductDialog} style={{ width: '32rem' }} breakpoints={{ '960px': '75vw', '641px': '90vw' }} header="Confirm" modal footer={deleteProductDialogFooter} onHide={hideDeleteProductDialog}>
                <div className="confirmation-content">
                    <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
                    {donor && (
                        <span>
                            Are you sure you want to delete <b>{donor.name}</b>?
                        </span>
                    )}
                </div>
            </Dialog> */}


            <Dialog visible={productDialog} style={{ width: '32rem' }} breakpoints={{ '960px': '75vw', '641px': '90vw' }} header="Donor Details" modal className="p-fluid" footer={productDialogFooter} onHide={hideDialog}>
                {donor.image && <img src={`./logo192.png`} alt={donor.image} className="donor-image block m-auto pb-3" />}

                <div className="formgrid grid">
                    
                <div className="field col">
                        <label htmlFor="name" className="font-bold">
                            First Name
                        </label>
                        <InputText id="firstName" value={donor.firstName} onChange={(e) => onInputChange(e, 'firstName')} required autoFocus className={classNames({ 'p-invalid': submitted && !donor.firstName })} />
                        {submitted && !donor.firstName && <small className="p-error">First Name is required.</small>}
                    </div> 
                    
                    <div className="field col">
                        <label htmlFor="name" className="font-bold">
                            Last Name
                        </label>
                        <InputText id="name" value={donor.lastName} onChange={(e) => onInputChange(e, 'lastName')} required autoFocus className={classNames({ 'p-invalid': submitted && !donor.lastName })} />
                        {submitted && !donor.lastName && <small className="p-error">Last Name is required.</small>}
                    </div>
                </div>
                  
                <div className="field">
                    <label htmlFor="name" className="font-bold">
                        Email
                    </label>
                    <InputText id="name" value={donor.email} onChange={(e) => onInputChange(e, 'email')} required autoFocus className={classNames({ 'p-invalid': submitted && !donor.email })} />
                    {submitted && !donor.email && <small className="p-error">Email is required.</small>}
                </div> 

                <div className="field">
                    <label htmlFor="phone" className="font-bold block mb-2">
                        Phone
                    </label>
                <InputText id="phone" 
                   value={donor.phone} onChange={(e) => onInputChange(e, 'phone')} required autoFocus className={classNames({ 'p-invalid': submitted && !donor.phone })} />
                    {submitted && !donor.phone && <small className="p-error">Phone is required.</small>}
                </div> 

                <div className="field">
                    <label htmlFor="name" className="font-bold">
                        Address
                    </label>
                    <InputText id="name" value={donor.address} onChange={(e) => onInputChange(e, 'address')} required autoFocus className={classNames({ 'p-invalid': submitted && !donor.address })} />
                    {submitted && !donor.address && <small className="p-error">Address is required.</small>}
                </div>  
            </Dialog>
        </>
    );
}

