#include "DataModel.h"

DataModel::DataModel(String^ codeText)
{
	_codeText = codeText;
}

DataModel::NavigateToAddCodeCommand::NavigateToAddCodeCommand(DataModel^ viewModel)
{
    _viewModel = viewModel;
}

bool DataModel::NavigateToAddCodeCommand::CanExecute(System::Object^ parameter) 
{
    return true;
}

void DataModel::NavigateToAddCodeCommand::Execute(System::Object^ parameter)
{
    _viewModel->ErrorsList->Clear();
    _viewModel->Methods = nullptr;
    _viewModel->Fields = nullptr;
    _viewModel->ObjectInstance = nullptr;
    _viewModel->Type = nullptr;

    try
    {
        auto code = _viewModel->CodeText;
        if (String::IsNullOrWhiteSpace(code))
        {
            _viewModel->ErrorsList->Add("No code to compile!");
            return;
        }

        auto result = VBCompilationHelper::VBCompiler::CompileVBCode(
            code,
            *_viewModel->SelectedOutputKind
        );

        if (!result->Success || result->Errors->Count > 0)
        {
            for each (auto error in result->Errors)
            {
                _viewModel->ErrorsList->Add(error);
            }
        }

        if (result->Success)
        {
            _viewModel->Type = result->FirstClassType;
            _viewModel->ObjectInstance = result->Instance;
            _viewModel->Methods = result->Methods;
            _viewModel->Fields = result->Fields;
        }
    }
    catch (System::Exception^ ex)
    {
        _viewModel->ErrorsList->Add(
            System::String::Format("Exception: {0}", ex->Message)
        );
    }
}

void DataModel::InvokeMethodCommandClass::Execute(System::Object^ parameter)
{
    try
    {
        _viewModel->MethodResult = "";
        
        if (_viewModel->SelectedMethod == nullptr || 
            _viewModel->SelectedMethod->Method == nullptr)
        {
            _viewModel->MethodResult = "Please select a method first!";
            return;
        }

        if (_viewModel->ObjectInstance == nullptr)
        {
            _viewModel->MethodResult = "No object instance available!";
            return;
        }

        auto method = _viewModel->SelectedMethod->Method;
        auto params = method->GetParameters();
        
        array<Object^>^ methodParams = nullptr;
        
        if (params->Length == 1)
        {
            methodParams = gcnew array<Object^>(1);
            methodParams[0] = _viewModel->MethodParameter;
        }
        else if (params->Length == 0)
        {
            methodParams = nullptr;
        }
        else
        {
            _viewModel->MethodResult = String::Format(
                "Method has {0} parameters. Only 0 or 1 parameter supported.",
                params->Length
            );
            return;
        }
        
        auto result = method->Invoke(_viewModel->ObjectInstance, methodParams);
        
        if (result != nullptr)
        {
            _viewModel->MethodResult = result->ToString();
        }
        else
        {
            _viewModel->MethodResult = "(void - no return value)";
        }
    }
    catch (System::Reflection::TargetInvocationException^ ex)
    {
        if (ex->InnerException != nullptr)
        {
            _viewModel->MethodResult = "Method error: " + ex->InnerException->Message;
        }
        else
        {
            _viewModel->MethodResult = "Method error: " + ex->Message;
        }
    }
    catch (Exception^ ex)
    {
        _viewModel->MethodResult = "Invoke error: " + ex->Message;
    }
}

void DataModel::UpdateCurrentFieldValue()
{
    try
    {
        if (_selectedField == nullptr || 
            _selectedField->Field == nullptr || 
            objectInstance == nullptr)
        {
            CurrentFieldValue = "";
            return;
        }

        auto value = _selectedField->Field->GetValue(objectInstance);
        CurrentFieldValue = (value != nullptr) ? value->ToString() : "(null)";
    }
    catch (Exception^ ex)
    {
        CurrentFieldValue = "Error: " + ex->Message;
    }
}

void DataModel::SetFieldCommandClass::Execute(System::Object^ parameter)
{
    try
    {
        if (_viewModel->SelectedField == nullptr)
        {
            System::Windows::MessageBox::Show("Please select a field first!");
            return;
        }

        if (_viewModel->ObjectInstance == nullptr)
        {
            System::Windows::MessageBox::Show("No object instance available!");
            return;
        }

        auto field = _viewModel->SelectedField->Field;
        auto newValue = _viewModel->NewFieldValue;

        Object^ convertedValue = nullptr;
        
        if (field->FieldType == String::typeid)
        {
            convertedValue = newValue;
        }
        else if (field->FieldType == int::typeid || field->FieldType == Int32::typeid)
        {
            int intValue;
            if (Int32::TryParse(newValue, intValue))
            {
                convertedValue = intValue;
            }
            else
            {
                System::Windows::MessageBox::Show("Invalid integer value!");
                return;
            }
        }
        else if (field->FieldType == bool::typeid || field->FieldType == Boolean::typeid)
        {
            bool boolValue;
            if (Boolean::TryParse(newValue, boolValue))
            {
                convertedValue = boolValue;
            }
            else
            {
                System::Windows::MessageBox::Show("Invalid boolean value! Use 'True' or 'False'.");
                return;
            }
        }
        else if (field->FieldType == double::typeid || field->FieldType == Double::typeid)
        {
            double doubleValue;
            if (Double::TryParse(newValue, doubleValue))
            {
                convertedValue = doubleValue;
            }
            else
            {
                System::Windows::MessageBox::Show("Invalid double value!");
                return;
            }
        }
        else
        {
            System::Windows::MessageBox::Show(
                String::Format("Type {0} is not supported yet!", field->FieldType->Name)
            );
            return;
        }

        field->SetValue(_viewModel->ObjectInstance, convertedValue);
        
        _viewModel->UpdateCurrentFieldValue();
        
        _viewModel->NewFieldValue = "";
        
        System::Windows::MessageBox::Show("Field value updated successfully!");
    }
    catch (Exception^ ex)
    {
        System::Windows::MessageBox::Show("Error setting field: " + ex->Message);
    }
}


