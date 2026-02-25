#include <windows.h>
using namespace System;

using namespace System::Reflection;
using namespace System::Security;
using namespace System::Runtime::Remoting;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;
using namespace System::ComponentModel;
using namespace System::Windows::Input;
using namespace Microsoft::CodeAnalysis;

typedef ObservableCollection<MethodInfo^> myList;

ref class MethodDisplay
{
public:
    property MethodInfo^ Method;
    property String^ DisplayText {
        String^ get() {
            if (Method == nullptr) return "";
            
            auto params = Method->GetParameters();
            auto paramList = gcnew System::Text::StringBuilder();
            
            for (int i = 0; i < params->Length; i++)
            {
                if (i > 0) paramList->Append(", ");
                paramList->Append(params[i]->ParameterType->Name);
                paramList->Append(" ");
                paramList->Append(params[i]->Name);
            }
            
            return String::Format("{0} {1}({2})",
                Method->ReturnType->Name,
                Method->Name,
                paramList->ToString()
            );
        }
    }

    MethodDisplay(MethodInfo^ method) {
        Method = method;
    }
};

ref class FieldDisplay
{
public:
    property FieldInfo^ Field;
    property String^ DisplayText {
        String^ get() {
            if (Field == nullptr) return "";
            
            return String::Format("{0} {1}",
                Field->FieldType->Name,
                Field->Name
            );
        }
    }

    FieldDisplay(FieldInfo^ field) {
        Field = field;
    }
};

ref class DataModel : INotifyPropertyChanged {

#pragma region Private Fields

	String^ _codeText;
	ICollection<String^>^ _errorsList = gcnew ObservableCollection<String^>();
	OutputKind^ selectedOutputKind = OutputKind::DynamicallyLinkedLibrary;
	array<MethodInfo^>^ methods = nullptr;
	array<FieldInfo^>^ fields = nullptr;
	ObservableCollection<MethodDisplay^>^ methodDisplays = gcnew ObservableCollection<MethodDisplay^>();
	ObservableCollection<FieldDisplay^>^ fieldDisplays = gcnew ObservableCollection<FieldDisplay^>();
	Object^ objectInstance;
	System::Type^ type;
	ObjectHandle^ handle;
	ICommand^ _AddCodeCommand;
	ICommand^ _InvokeMethodCommand;
	ICommand^ _SetFieldCommand;
	
	MethodDisplay^ _selectedMethod;
	String^ _methodParameter;
	String^ _methodResult;
	
	FieldDisplay^ _selectedField;
	String^ _currentFieldValue;
	String^ _newFieldValue;

#pragma endregion End of Private Fields

private:
	void OnPropertyChanged(String^ propertyName) {
		PropertyChanged(this, gcnew PropertyChangedEventArgs(propertyName));
	}
	
	void UpdateCurrentFieldValue();

public:

#pragma region Public Constructors

	DataModel(String^ codeText);

#pragma endregion End of Public Constructors

#pragma region Public Events

	virtual event System::ComponentModel::PropertyChangedEventHandler^ PropertyChanged;

#pragma endregion End of Public Events

#pragma region Public Properties

	property String^ CodeText {
		String^ get() {
			return _codeText;
		}

		void set(String^ value) {
			if (_codeText != value) {
				_codeText = value;
				OnPropertyChanged("CodeText");
			}
		}
	}

	property ICollection<String^>^ ErrorsList {
		ICollection<String^>^ get() {
			return _errorsList;
		}

		void set(ICollection<String^>^ value) {
			_errorsList = value;
		}
	}

	property ICommand^ AddCodeCommand {
		ICommand^ get() {
			if (_AddCodeCommand == nullptr)
			{
				_AddCodeCommand = gcnew NavigateToAddCodeCommand(this);
			}
			return _AddCodeCommand;
		}
		void set(ICommand^ value) {
			_AddCodeCommand = value;
		}
	}

	property ICommand^ InvokeMethodCommand {
		ICommand^ get() {
			if (_InvokeMethodCommand == nullptr)
			{
				_InvokeMethodCommand = gcnew InvokeMethodCommandClass(this);
			}
			return _InvokeMethodCommand;
		}
	}
	
	property ICommand^ SetFieldCommand {
		ICommand^ get() {
			if (_SetFieldCommand == nullptr)
			{
				_SetFieldCommand = gcnew SetFieldCommandClass(this);
			}
			return _SetFieldCommand;
		}
	}
	
	property OutputKind^ SelectedOutputKind {
		OutputKind^ get() {
			return selectedOutputKind;
		}

		void set(OutputKind^ value) {
			selectedOutputKind = value;
			OnPropertyChanged("SelectedOutputKind");
		}
	}
	
	property array<MethodInfo^>^ Methods {
		array<MethodInfo^>^ get() {
			return methods;
		}

		void set(array<MethodInfo^>^ value) {
			if (methods != value) {
				methods = value;
				
				methodDisplays->Clear();
				if (methods != nullptr) {
					for each (auto method in methods) {
						methodDisplays->Add(gcnew MethodDisplay(method));
					}
				}
				
				OnPropertyChanged("Methods");
				OnPropertyChanged("MethodDisplays");
			}
		}
	}

	property array<FieldInfo^>^ Fields {
		array<FieldInfo^>^ get() {
			return fields;
		}

		void set(array<FieldInfo^>^ value) {
			if (fields != value) {
				fields = value;
				
				fieldDisplays->Clear();
				if (fields != nullptr) {
					for each (auto field in fields) {
						fieldDisplays->Add(gcnew FieldDisplay(field));
					}
				}
				
				OnPropertyChanged("Fields");
				OnPropertyChanged("FieldDisplays");
			}
		}
	}

	property ObservableCollection<MethodDisplay^>^ MethodDisplays {
		ObservableCollection<MethodDisplay^>^ get() {
			return methodDisplays;
		}
	}

	property ObservableCollection<FieldDisplay^>^ FieldDisplays {
		ObservableCollection<FieldDisplay^>^ get() {
			return fieldDisplays;
		}
	}

	property MethodDisplay^ SelectedMethod {
		MethodDisplay^ get() {
			return _selectedMethod;
		}
		void set(MethodDisplay^ value) {
			_selectedMethod = value;
			OnPropertyChanged("SelectedMethod");
		}
	}

	property String^ MethodParameter {
		String^ get() {
			return _methodParameter;
		}
		void set(String^ value) {
			_methodParameter = value;
			OnPropertyChanged("MethodParameter");
		}
	}

	property String^ MethodResult {
		String^ get() {
			return _methodResult;
		}
		void set(String^ value) {
			_methodResult = value;
			OnPropertyChanged("MethodResult");
		}
	}
	
	property FieldDisplay^ SelectedField {
		FieldDisplay^ get() {
			return _selectedField;
		}
		void set(FieldDisplay^ value) {
			_selectedField = value;
			OnPropertyChanged("SelectedField");
			UpdateCurrentFieldValue();
		}
	}

	property String^ CurrentFieldValue {
		String^ get() {
			return _currentFieldValue;
		}
		void set(String^ value) {
			_currentFieldValue = value;
			OnPropertyChanged("CurrentFieldValue");
		}
	}

	property String^ NewFieldValue {
		String^ get() {
			return _newFieldValue;
		}
		void set(String^ value) {
			_newFieldValue = value;
			OnPropertyChanged("NewFieldValue");
		}
	}

	property Object^ ObjectInstance {
		Object^ get() {
			return objectInstance;
		}

		void set(Object^ value) {
			objectInstance = value;
		}
	}

	property ObjectHandle^ Handle {
		ObjectHandle^ get() {
			return handle;
		}

		void set(ObjectHandle^ value) {
			handle = value;
		}
	}

	property System::Type^ Type {
		System::Type^ get() {
			return type;
		}

		void set(System::Type^ value) {
			type = value;
		}
	}

#pragma endregion End of Public Properties

#pragma region Public Nested Classes

	ref class NavigateToAddCodeCommand : public ICommand {
		DataModel^ _viewModel;
	public:
		NavigateToAddCodeCommand(DataModel^ viewModel);
		virtual bool CanExecute(System::Object^ parameter) = ICommand::CanExecute;
		virtual void Execute(System::Object^ parameter) = ICommand::Execute;
		virtual event EventHandler^ CanExecuteChanged {
			void add(EventHandler^) {}
			void remove(EventHandler^) {}
		}
	};

	ref class InvokeMethodCommandClass : public ICommand {
		DataModel^ _viewModel;
	public:
		InvokeMethodCommandClass(DataModel^ viewModel) {
			_viewModel = viewModel;
		}

		virtual bool CanExecute(System::Object^ parameter) = ICommand::CanExecute {
			return true;
		}

		virtual void Execute(System::Object^ parameter) = ICommand::Execute;

		virtual event EventHandler^ CanExecuteChanged {
			void add(EventHandler^) {}
			void remove(EventHandler^) {}
		}
	};
	
	ref class SetFieldCommandClass : public ICommand {
		DataModel^ _viewModel;
	public:
		SetFieldCommandClass(DataModel^ viewModel) {
			_viewModel = viewModel;
		}

		virtual bool CanExecute(System::Object^ parameter) = ICommand::CanExecute {
			return true;
		}

		virtual void Execute(System::Object^ parameter) = ICommand::Execute;

		virtual event EventHandler^ CanExecuteChanged {
			void add(EventHandler^) {}
			void remove(EventHandler^) {}
		}
	};

#pragma endregion End of Public Nested Classes

};

